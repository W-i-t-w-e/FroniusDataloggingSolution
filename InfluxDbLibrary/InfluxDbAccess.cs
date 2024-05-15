using FroniusDataLoggerShared.Models;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InfluxDbLibrary
{
    public class InfluxDbAccess : IDisposable
    {
        private readonly ILogger<InfluxDbAccess> logger;
        private readonly string accessToken;
        private readonly string url;
        private readonly string bucket;
        private readonly string org;

        private CancellationTokenSource cts;

        public InfluxDbAccess(ILogger<InfluxDbAccess> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.accessToken = configuration.GetValue<string>("Database:Token");
            this.url = configuration.GetValue<string>("Database:Url");
            this.bucket = configuration.GetValue<string>("Database:Bucket");
            this.org = configuration.GetValue<string>("Database:Organization");
            cts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            logger.LogDebug("Disposing InfluxDbAccess...");
            cts?.Cancel();
            cts?.Dispose();
        }

        public async Task SaveDataAsync(ResultModel response)
        {
            logger.LogDebug("Create the client");
            using var client = new InfluxDBClient(url, accessToken);

            logger.LogDebug("Create the writer");
            var writeApiAsync = client.GetWriteApiAsync();

            logger.LogDebug("Create the data point");
            var point = PointData.Measurement(response.Command.ToString())
                                 .Tag("DeviceId", response.DeviceId.ToString())
                                 .Tag("DeviceModel", response.DeviceModel.ToString())
                                 .Field("Result", response.Result)
                                 .Timestamp(response.TimeStamp.ToUniversalTime(), WritePrecision.S);

            logger.LogDebug($"write DeviceId:{response.DeviceId}, DeviceModel:{response.DeviceModel}, QueryType:{response.Command}, Result:{response.Result}, TimeStamp:{response.TimeStamp.ToUniversalTime()} to the database. ");
            await writeApiAsync.WritePointAsync(point, bucket, org, cts.Token);

        }
    }
}
