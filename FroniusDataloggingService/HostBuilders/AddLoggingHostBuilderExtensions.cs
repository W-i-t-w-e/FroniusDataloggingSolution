using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FroniusDataLoggerService.HostBuilders
{
    public static class AddLoggingHostBuilderExtensions
    {
        public static IHostBuilder AddLogging(this IHostBuilder host)
        {
            host.ConfigureLogging((context, b) =>
            {
                var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(context.Configuration)
                .CreateLogger();

                b.AddSerilog(logger);
            });
            return host;
        }
    }
}
