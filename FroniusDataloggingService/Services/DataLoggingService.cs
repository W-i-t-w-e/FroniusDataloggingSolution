using FroniusDataLoggerShared.Models;
using FroniusInterfaceCardLibrary.Helpers;
using FroniusInterfaceCardLibrary.Serial;
using InfluxDbLibrary;

namespace FroniusDataLoggerService.Services
{
    public class DataLoggingService : IDisposable
    {
        private readonly ILogger<DataLoggingService> logger;
        private readonly ComPortAccess serial;
        private readonly InfluxDbAccess db;
        private readonly object locker;
        private EventWaitHandle eventWaitHandle;
        private List<DeviceModel> localNetDevices;
        private Queue<ResponseModel> responseQueue;
        private CancellationTokenSource cts;
        public event Action<string> ReportErrorEvent;
        public DataLoggingService(ILogger<DataLoggingService> logger, ComPortAccess serial, InfluxDbAccess db)
        {
            this.logger = logger;
            this.serial = serial;
            this.db = db;
            locker = new object();
            localNetDevices = new();
            responseQueue = new();
            eventWaitHandle = new AutoResetEvent(false);
            cts = new CancellationTokenSource();
            this.serial.DataReceived += OnDataReceived;
            Task.Run(async () => await StartDequeueResponsesAsync(), cts.Token);

        }

        public void QueryData()
        {
            if (serial.IsConnected == false)
            {
                serial.Connect();
            }
            if (localNetDevices.Count == 0)
            {
                // Query for devices
                GetActiveInverterFromInterface();
            }
            // Get data from every device
            foreach (var device in localNetDevices)
            {
                QueryDataFromDevice(device);
            }
        }

        public void Dispose()
        {
            logger.LogDebug("Stopping DataLoggingService...");
            serial.Disconnect();
            serial.DataReceived -= OnDataReceived;
            cts.Cancel();
            cts.Dispose();
            logger.LogDebug("DataLoggingService stopped...");
        }

        private async Task StartDequeueResponsesAsync()
        {
            try
            {
                while (cts.Token.IsCancellationRequested == false)
                {
                    ResponseModel? response = null;

                    lock (locker)
                    {
                        if (responseQueue.Count > 0)
                        {
                            response = responseQueue.Dequeue();
                            if (response is null)
                            {
                                return;
                            }
                        }
                    }
                    if (response is not null)
                    {
                        await HandleResponseAsync(response);
                        continue;
                    }

                    // nothing left in queue -> wait for signal
                    eventWaitHandle.WaitOne();
                }

            }
            catch (OperationCanceledException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        private async Task HandleResponseAsync(ResponseModel response)
        {
            switch (response.Device)
            {
                case DeviceType.InterfaceCard:
                    HandleInterfaceCardData(response);
                    break;

                case DeviceType.Inverter:
                    await HandleInverterDataAsync(response);
                    break;

                default:
                    logger.LogWarning($"Device {response.Device} not yet implemented...");
                    break;
            }
        }

        private void OnDataReceived(ResponseModel response)
        {
            // This statement is secured by lock to prevent other threads to mess with queue while enqueuing staffs
            lock (locker) responseQueue.Enqueue(response);

            // Signal worker that model is enqueued and that it can be processed
            eventWaitHandle.Set();
        }

        private void HandleInterfaceCardData(ResponseModel response)
        {
            if (response.Length == 0)
            {
                logger.LogDebug($"No Data received");
                return;
            }

            switch (response.Command)
            {
                // *First run* get the active inverters
                case QueryCommand.GetActiveInverterNumbers:

                    localNetDevices.Clear();
                    // Add all connected inverters to list
                    response.Data.ForEach(x => localNetDevices.Add(new DeviceModel { Id = x }));

                    // Get the model of each inverter
                    localNetDevices.ForEach(x => serial.Write(ByteHelpers.SetSendSequence(QueryCommand.GetDevice, DeviceType.Inverter, x.Id)));
                    break;
                case QueryCommand.ErrorMessage:
                    HandleErrorMessage(response);
                    break;
                default:
                    logger.LogWarning($"Value: {response.Command} not yet handled in HandleInterfaceCardData...");
                    break;

            }
        }

        private async Task HandleInverterDataAsync(ResponseModel response)
        {
            switch (response.Command)
            {
                case QueryCommand.ErrorMessage:
                    HandleErrorMessage(response);
                    break;

                case QueryCommand.GetDevice:
                    SetDeviceModel(response);
                    break;

                default:
                    var result = ParseResponse(response);
                    // This statement is secured by lock to prevent other threads to mess with queue while enqueuing staffs
                    await db.SaveDataAsync(result);
                    break;
            }
        }

        private void SetDeviceModel(ResponseModel response)
        {
            // Get the sending device from the list
            var device = localNetDevices.FirstOrDefault(d => d.Id == response.Number);
            // if found -> update Model and Type
            if (device is not null)
            {
                device.Type = response.Device;
                device.Model = (DeviceType)response.Data.FirstOrDefault();

                logger.LogDebug($"Device Model is: {device.Model}");
            }
        }

        /// <summary>
        /// Calculate measured values and check if signed or unsigned data type
        /// NOTE: 1phase inverters only provide unsigned data types
        /// </summary>
        /// <param name="data"></param>
        private ResultModel ParseResponse(ResponseModel response)
        {
            // A new result
            var output = new ResultModel();
            // The device Id from this response
            output.DeviceId = response.Number;
            // Get the device model from this response
            output.DeviceModel = localNetDevices.FirstOrDefault(x => x.Id == response.Number)?.Model ?? DeviceType.NotSet;
            // The command from this response
            output.Command = response.Command;

            // A value for calculation
            var unionValue = new ValueUnion();
            //+++ get exponent:  
            double byDataInX;
            switch (response.Data[2])
            {
                case 0xFF: byDataInX = -1; break;
                case 0xFE: byDataInX = -2; break;
                case 0xFD: byDataInX = -3; break;
                case 0xFC: byDataInX = -4; break;
                case 0xFB: byDataInX = -5; break;
                case 0xFA: byDataInX = -6; break;
                case 0xF9: byDataInX = -7; break;
                case 0xF8: byDataInX = -8; break;
                case 0xF7: byDataInX = -9; break;
                case 0xF6: byDataInX = -10; break;
                case 0xF5: byDataInX = -11; break;
                case 0xF4: byDataInX = -12; break;
                case 0xF3: byDataInX = -13; break;
                default: byDataInX = response.Data[2]; break;
            }
            // calculate decimal datavalue - actual approach
            //+++ get MSB/LSB-value
            unionValue.ValueUpperByte = response.Data[0];
            unionValue.ValueLowerByte = response.Data[1];

            //+++ signed data values(e.g. temperatures)
            if (response.Command == QueryCommand.GetAmbientTemperature ||
                response.Command == QueryCommand.GetTempCh1Now ||
                response.Command == QueryCommand.GetTempCh2Now ||
                (response.Command >= QueryCommand.GetInsolationNow && response.Command <= QueryCommand.GetMaxTempCh2Total))
            {   //+++ calculate signed value and round to 2 decimal places
                output.Result = Math.Round(unionValue.ValueUnsigned * Math.Pow(10, byDataInX), 2);
                return output;
            }
            //+++ unsigned data values(all others)
            //+++ calculate unsigned value and round to 2 decimal places
            output.Result = Math.Round(unionValue.ValueSigned * Math.Pow(10, byDataInX), 2);
            return output;

        }

        private void HandleErrorMessage(ResponseModel response)
        {
            var errorMessage = string.Empty;
            try
            {
                errorMessage = $"DeviceID: {response.Number}, DeviceModel: {localNetDevices.FirstOrDefault(x => x.Id == response.Number)?.Model ?? DeviceType.NotSet}, originating Command: {(QueryCommand)response.Data[0]}, ErrorMessage: {(ErrorType)response.Data[1]}";
            }
            // if no valid errortype
            catch (Exception)
            {
                errorMessage = "Error while parsing returned error";
            }
            logger.LogError(errorMessage);
            ReportErrorEvent?.Invoke(errorMessage);
            // Cleanup the device list to enforce query for devices
            localNetDevices.Clear();
        }

        private void QueryDataFromDevice(DeviceModel device)
        {
            var getActualPower = ByteHelpers.SetSendSequence(QueryCommand.GetPowerNow, device.Type, device.Id);
            serial.Write(getActualPower);

            var acCurrent = ByteHelpers.SetSendSequence(QueryCommand.GetAcCurrentNow, device.Type, device.Id);
            serial.Write(acCurrent);

            var acVoltage = ByteHelpers.SetSendSequence(QueryCommand.GetAcVoltageNow, device.Type, device.Id);
            serial.Write(acVoltage);

            var dcCurrent = ByteHelpers.SetSendSequence(QueryCommand.GetDcCurrentNow, device.Type, device.Id);
            serial.Write(dcCurrent);

            var dcVoltage = ByteHelpers.SetSendSequence(QueryCommand.GetDcVoltageNow, device.Type, device.Id);
            serial.Write(dcVoltage);

            var maxDcVoltageDay = ByteHelpers.SetSendSequence(QueryCommand.GetMaxDcVoltageDay, device.Type, device.Id);
            serial.Write(maxDcVoltageDay);

            var maxAcVoltageDay = ByteHelpers.SetSendSequence(QueryCommand.GetMaxAcVoltageDay, device.Type, device.Id);
            serial.Write(maxAcVoltageDay);

            var energyDay = ByteHelpers.SetSendSequence(QueryCommand.GetEnergyDay, device.Type, device.Id);
            serial.Write(energyDay);

            var maxPowerDay = ByteHelpers.SetSendSequence(QueryCommand.GetMaxPowerDay, device.Type, device.Id);
            serial.Write(maxPowerDay);
        }

        private void GetActiveInverterFromInterface()
        {
            var GetActiveInverterNumbers = ByteHelpers.SetSendSequence(QueryCommand.GetActiveInverterNumbers, DeviceType.InterfaceCard, 0);
            serial.Write(GetActiveInverterNumbers);
        }
    }
}
