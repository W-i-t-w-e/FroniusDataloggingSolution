using FroniusDataLoggerService.Services;
using FroniusInterfaceCardLibrary.Serial;
using InfluxDbLibrary;
using SendMailLibrary;

namespace FroniusDataLoggerService.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices((context, services) => 
            {
                services.AddHostedService<Worker>();
                services.AddTransient<SendMail>();            
                services.AddTransient<DataLoggingService>();
                services.AddTransient<ComPortAccess>();
                services.AddTransient<InfluxDbAccess>();
                
            });
            return host;
        }


    }
}
