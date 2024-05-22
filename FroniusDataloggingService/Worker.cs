using FroniusDataLoggerService.Services;
using Innovative.Geometry;
using Innovative.SolarCalculator;
using SendMailLibrary;

namespace FroniusDataLoggerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IHostApplicationLifetime hostApplicationLifetime;
        private readonly DataLoggingService dataLogger;
        private readonly SendMail mail;
        private DateTime lastErrorMailSend;
        private TimeSpan errorMailInterval;
        private PeriodicTimer timer;
        private Angle latitude;
        private Angle longitude;
        private string timeZoneId;

        public Worker(ILogger<Worker> logger, IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration, DataLoggingService dataLogger, SendMail mail)
        {
            this.logger = logger;
            this.hostApplicationLifetime = hostApplicationLifetime;
            this.dataLogger = dataLogger;
            this.mail = mail;
            latitude = configuration.GetValue<double>("Geo:Latitude");
            longitude = configuration.GetValue<double>("Geo:Longitude");
            timeZoneId = configuration.GetValue<string>("Geo:TimeZoneId");
            var timeSpan = configuration.GetValue<TimeSpan>("Common:PollInterval");
            timer = new PeriodicTimer(timeSpan);
            lastErrorMailSend = DateTime.MinValue;
            dataLogger.ReportErrorEvent += async (s) => await OnReportErrorAsync(s);
        }

        private async Task OnReportErrorAsync(string errorMessage)
        {
            if (ShouldSendEmail())
            {
                await SendMailAsync("Fehler im Datalogger", errorMessage);
                lastErrorMailSend = DateTime.Now;
            }
        }

        /// <summary>
        /// Only one mail within a specified timespan and only between sunrise and sunset
        /// with a lag of 30 minutes
        /// </summary>
        /// <returns></returns>
        private bool ShouldSendEmail()
        {
            return lastErrorMailSend <= DateTime.Now.Add(-errorMailInterval) &&
                   IsBetweenSunriseAndSunset(30);
        }

        private bool IsBetweenSunriseAndSunset(int lag)
        {
            return DateTime.Now.TimeOfDay > GetSunrise().AddMinutes(lag).TimeOfDay && DateTime.Now.TimeOfDay < GetSunset().AddMinutes(-lag).TimeOfDay;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken) && stoppingToken.IsCancellationRequested == false)
                {
                    if (IsBetweenSunriseAndSunset(0))
                    {
                        dataLogger.QueryData();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                await SendMailAsync("Fehler im FroniusDataLoggerService", $"{ex.Source}: {ex.Message}");
                hostApplicationLifetime.StopApplication();
            }

        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Stopping the data logging service ...");
            timer.Dispose();
            await base.StopAsync(cancellationToken);
            logger.LogInformation($"Services stopped at {DateTime.Now}");
            return;
        }

        private DateTime GetSunrise()
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            // https://www.latlong.net/
            var solarTimes = new SolarTimes(DateTime.Now.Date, latitude, longitude);
            return TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunrise.ToUniversalTime(), timeZone);
        }

        private DateTime GetSunset()
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            // https://www.latlong.net/
            var solarTimes = new SolarTimes(DateTime.Now.Date, latitude, longitude);
            return TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunset.ToUniversalTime(), timeZone);
        }

        private async Task SendMailAsync(string subject, string body)
        {
            try
            {
                await mail.SendEmailAsync(subject, body);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in SendMail: {ex.Message}");
            }
        }
    }
}
