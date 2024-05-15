using FroniusDataLoggerShared.Models;
using FroniusInterfaceCardLibrary.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO.Ports;

namespace FroniusInterfaceCardLibrary.Serial
{
    public class ComPortAccess
    {
        private readonly ILogger<ComPortAccess> logger;

        private readonly string serialPortName;
        private readonly int baudRate;
        private readonly Parity parity;
        private readonly int dataBits;
        private readonly StopBits stopBits;
        private readonly int readBufferSize;
        private readonly bool isDtrEnabled;
        private readonly byte[] startSequence;
        
        private SerialPort serialPort;
        private byte[] data;
        private CancellationTokenSource cts;

        public bool IsConnected => serialPort is not null && serialPort.IsOpen;
        public event Action<ResponseModel> DataReceived;

        public ComPortAccess(ILogger<ComPortAccess> logger, IConfiguration configuration)
        {
            this.logger = logger;
            data = Array.Empty<byte>();
            startSequence = new byte[] { 128, 128, 128 };
            readBufferSize = 150;
            isDtrEnabled = false;
            cts = new CancellationTokenSource();

            serialPortName = configuration.GetValue<string>("Serial:ComPort");
            baudRate = configuration.GetValue<int>("Serial:BaudRate");
            parity = (Parity)configuration.GetValue<int>("Serial:Parity");
            dataBits = configuration.GetValue<int>("Serial:DataBits");
            stopBits = (StopBits)configuration.GetValue<int>("Serial:StopBits");
        }

        public void Connect()
        {
            serialPort = new SerialPort(serialPortName, baudRate, parity, dataBits, stopBits);
            serialPort.ReadBufferSize = readBufferSize;
            serialPort.DtrEnable = isDtrEnabled;
            serialPort.Open();

            if (IsConnected == false)
            {
                throw new IOException($"Could not establish a connection to the serial port: {serialPortName}");
            }

            logger.LogInformation($"Serial port {serialPortName} successfully connected");

            // Start reading for incomming data
            Task.Run(() => Read(), cts.Token);
        }

        public void Disconnect()
        {
            logger.LogDebug("Disconnecting serial port...");
            cts.Cancel();
            cts.Dispose();

            if (serialPort is null)
            {
                return;
            }
            serialPort.Close();
            serialPort.Dispose();
            logger.LogInformation($"SerialPort {serialPort.PortName} has been closed.");
        }

        #region Send Method

        public void Write(byte[] sendByte)
        {
            serialPort.Write(sendByte, 0, sendByte.Length);
        }

        #endregion

        #region Receive Method

        private void Read()
        {

            while ((!cts.IsCancellationRequested) && (IsConnected))
            {
                // Get the next byte from the comport
                var newByte = (byte)serialPort.ReadByte();

                NewByteFromSerial(newByte);
            }
        }

        #endregion

        #region DataHandling Methods

        /// <summary>
        /// A method to process incomming byte from serial to a full localnet sequence
        /// </summary>
        /// <param name="newByte"></param>
        private void NewByteFromSerial(byte newByte)
        {
            logger.LogDebug($"Incomming Byte: {newByte}");
            int seqIndex;
            int dataLength;
            byte[] destArray;
            byte[] remainArray;
            logger.LogDebug($"ArrayLength before add: {data.Length}");
            // Add the byte to the array
            data = ByteHelpers.AddByteToArray(data, newByte);
            logger.LogDebug($"ArrayLength after add: {data.Length}");

            // Get the index of the next sequence
            seqIndex = data.GetIndexOfSequence(startSequence);

            // Stop if no sequence found
            if (seqIndex == -1)
            {
                return;
            }

            // Get length of data bytes
            dataLength = data[seqIndex + 3];

            logger.LogDebug($"DataLength: {dataLength}");

            // create an array to fit the found sequence
            destArray = new byte[8 + dataLength];

            logger.LogDebug($"ArrayLength: {data.Length}, DestArrayLength: {destArray.Length}");
            logger.LogDebug($"ArrayLength starting from startindex {data.Length - seqIndex}");
            // Check if the array is complete
            // NOTE: data Length needs to be calculated starting at the seqindex
            if (data.Length - seqIndex < destArray.Length)
            {
                // Return if incomplete
                return;
            }
            // copy the sequence from the sourcearray to the destarray
            // NOTE: starting at the index found, with the length of the destarray
            logger.LogDebug($"copy bytearray with length {data.Length}, from the sequence at index: {seqIndex}, to the destination array with a length of {destArray.Length}");

            Array.Copy(data, seqIndex, destArray, 0, destArray.Length);

            // Process received sequence
            ParseSerialData(destArray);

            logger.LogDebug($"Create a Array with the remaining data: with a size of \"{data.Length}\" minus the index of the startsequence \"{seqIndex}\" minus the length of the destination array \"{destArray.Length}\" = \"{data.Length - seqIndex - destArray.Length}\"");

            // Create a new array with the length of the remaining bytes from the sourcearray
            // NOTE: the length of the sourcearray - the bytes till the start of the next sequence - the length of the found sequence
            remainArray = new byte[data.Length - seqIndex - destArray.Length];

            logger.LogDebug($"copy bytearray with length {data.Length}, from the sequence at index: {seqIndex} plus the destArrayLength \"{destArray.Length}\", to the remainArray with a length of {remainArray.Length}");
            //update the source array with the found sequence removed
            // NOTE: sourceIndex = the index of the found sequence + the length of the found sequence
            Array.Copy(data, seqIndex + destArray.Length, remainArray, 0, remainArray.Length);

            // Set the sourcearray to the remaining bytes
            data = remainArray;
        }

        private void ParseSerialData(byte[] bytesReceived)
        {
            // Validate the checksum
            if (!IsValidCheckSum(bytesReceived))
            {
                return;
            }

            var receivedData = new ResponseModel();

            //--- Detect DataFieldLength (0..127) (Byte 4)
            receivedData.Length = bytesReceived[3];

            // Log the fieldlength
            logger.LogDebug($"FieldLength is: {receivedData.Length}");

            //--- Detect Device (Byte 5)

            try
            {
                receivedData.Device = (DeviceType)bytesReceived[4];
                // Log the device
                logger.LogDebug($"Device is: {receivedData.Device}");

            }
            catch (Exception)
            {
                logger.LogWarning($"Unknown Device: {bytesReceived[4].ByteToString()}");
                return;
            }


            //--- Detect Number of device (Byte 6)
            receivedData.Number = bytesReceived[5];
            // Log the device number
            logger.LogDebug($"Devicenumber is {receivedData.Number}");


            //--- Detect Command (Byte 7)
            receivedData.Command = (QueryCommand)bytesReceived[6];
            // Log the command
            logger.LogDebug($"Command query {receivedData.Command}");

            //--- Detect Data (Byte 8 and up!)

            if (bytesReceived.Length > 8)
            {
                // Set the startindex of the available data
                var startIndex = 7;
                // Because startindex counts as one, length should be decreased by one
                var endIndex = startIndex + receivedData.Length - 1;
                for (int i = startIndex; i <= endIndex; i++)
                {
                    // should always be true, because the overall arraylength should be large enough (just for assurance)
                    if (i < bytesReceived.Length - 1)
                        // Add each datafield to the list
                        receivedData.Data.Add(bytesReceived[i]);
                }

            }
            // Notify new data received
            DataReceived?.Invoke(receivedData);
        }

        #endregion

        #region Helper Methods

        private bool IsValidCheckSum(byte[] bytesReceived)
        {
            try
            {
                byte checkSum;

                // Add datafield to checksum calculation
                checkSum = bytesReceived[3];

                // Add device to checksum calculation
                checkSum += bytesReceived[4];

                // Add device number to checksum calculation
                checkSum += bytesReceived[5];

                // Add command to checksum calculation
                checkSum += bytesReceived[6];

                // Add data to checksum calculation if any
                // Note: if array is smaller than 8 -> don't add data to checksum...
                if (bytesReceived.Length > 8)
                {
                    // Set the startindex of the available data
                    var startIndex = 7;
                    // Set the endindex of the available data
                    // Note index is 0 based, Length 1 based -> last index is checksum, so substract 2
                    var endIndex = bytesReceived.Length - 2;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        checkSum += bytesReceived[i];
                    }
                }
                // Get the last received byte as the checksum
                var checkSumIndex = bytesReceived.Length - 1;

                var receiveChecksum = bytesReceived[checkSumIndex];

                // Check if calculated value matches the received value
                if (checkSum != receiveChecksum)
                {
                    logger.LogError($"Checksum Mismatch! Calculated: {checkSum}, Received: {receiveChecksum}");
                    // return mismatch
                    return false;
                }
                // return match
                return true;
            }
            catch (Exception)
            {
                logger.LogError($"Invalid byte array for checksum calculation");
                return false;
            }
        }

        #endregion
    }
}
