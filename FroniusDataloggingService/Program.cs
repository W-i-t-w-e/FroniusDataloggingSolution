using FroniusDataLoggerService.HostBuilders;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .AddLogging()
    .AddServices()
    .UseSystemd()
    .Build();


var logger = host.Services.GetRequiredService<ILogger<Program>>();
try
{
    // ASCII generator at https://de.rakko.tools/tools/68/ font: big
    var appVersion = Assembly.GetExecutingAssembly().GetName().Version;

    logger.LogInformation("");
    logger.LogInformation("");
    logger.LogInformation("");
    logger.LogInformation("");
    logger.LogInformation("");
    logger.LogInformation("");
    logger.LogInformation("");
    logger.LogInformation("");
    logger.LogInformation("");
    logger.LogInformation("");
    logger.LogInformation("");
    logger.LogInformation(@"  ______                      _ ");
    logger.LogInformation(@" |  ____|                    (_) ");
    logger.LogInformation(@" | |__    _ __   ___   _ __   _  _   _  ___ ");
    logger.LogInformation(@" |  __|  | '__| / _ \ | '_ \ | || | | |/ __| ");
    logger.LogInformation(@" | |     | |   | (_) || | | || || |_| |\__ \ ");
    logger.LogInformation(@" |_|     |_|    \___/ |_| |_||_| \__,_||___/ ");
    logger.LogInformation(@"  _____         _                 __                       _____                   _ ");
    logger.LogInformation(@" |_   _|       | |               / _|                     / ____|                 | | ");
    logger.LogInformation(@"   | |   _ __  | |_   ___  _ __ | |_   __ _   ___   ___  | |       __ _  _ __   __| | ");
    logger.LogInformation(@"   | |  | '_ \ | __| / _ \| '__||  _| / _` | / __| / _ \ | |      / _` || '__| / _` | ");
    logger.LogInformation(@"  _| |_ | | | || |_ |  __/| |   | |  | (_| || (__ |  __/ | |____ | (_| || |   | (_| | ");
    logger.LogInformation(@" |_____||_| |_| \__| \___||_|   |_|   \__,_| \___| \___|  \_____| \__,_||_|    \__,_| ");
    logger.LogInformation("");
    logger.LogInformation(@"  _____          _           _ ");
    logger.LogInformation(@" |  __ \        | |         | | ");
    logger.LogInformation(@" | |  | |  __ _ | |_   __ _ | |  ___    __ _   __ _   ___  _ __ ");
    logger.LogInformation(@" | |  | | / _` || __| / _` || | / _ \  / _` | / _` | / _ \| '__| ");
    logger.LogInformation(@" | |__| || (_| || |_ | (_| || || (_) || (_| || (_| ||  __/| | ");
    logger.LogInformation(@" |_____/  \__,_| \__| \__,_||_| \___/  \__, | \__, | \___||_| ");
    logger.LogInformation(@"                                        __/ |  __/ | ");
    logger.LogInformation(@"                                       |___/  |___/ ");
    logger.LogInformation("");
    logger.LogInformation(@" ################################################################################################## ");
    logger.LogInformation(@"#  _               _    _                                  __          __              _  _        # ");
    logger.LogInformation(@"# | |             | |  | |                                 \ \        / /             | || |       # ");
    logger.LogInformation(@"# | |__   _   _   | |__| |  __ _  _ __   _ __    ___  ___   \ \  /\  / /   ___    ___ | || | __    # ");
    logger.LogInformation(@"# | '_ \ | | | |  |  __  | / _` || '_ \ | '_ \  / _ \/ __|   \ \/  \/ /   / _ \  / _ \| || |/ /    # ");
    logger.LogInformation(@"# | |_) || |_| |  | |  | || (_| || | | || | | ||  __/\__ \    \  /\  /   | (_) ||  __/| ||   <     # ");
    logger.LogInformation(@"# |_.__/  \__, |  |_|  |_| \__,_||_| |_||_| |_| \___||___/     \/  \/     \___/  \___||_||_|\_\    # ");
    logger.LogInformation(@"#          __/ |                                                                                   # ");
    logger.LogInformation(@"#         |___/                                                                                    # ");
    logger.LogInformation(@"#                       ___   ____      __  ___      ___    ___   ___   ___                        # ");
    logger.LogInformation(@"#                      |__ \ |___ \    /_ ||__ \    |__ \  / _ \ |__ \ |__ \                       # ");
    logger.LogInformation(@"#                         ) |  __) |    | |   ) |      ) || | | |   ) |   ) |                      # ");
    logger.LogInformation(@"#                        / /  |__ <     | |  / /      / / | | | |  / /   / /                       # ");
    logger.LogInformation(@"#                       / /_  ___) | _  | | / /_  _  / /_ | |_| | / /_  / /_                       # ");
    logger.LogInformation(@"#                      |____||____/ (_) |_||____|(_)|____| \___/ |____||____|                      # ");
    logger.LogInformation(@"#                                                                                                  # ");
    logger.LogInformation(@" ################################################################################################## ");
    logger.LogInformation("");
    logger.LogInformation("");
    logger.LogInformation(" ###########################################");
    logger.LogInformation("#                                           #");
    logger.LogInformation($"#----->Starting up Service Version {appVersion.Major}.{appVersion.Minor}<-----#");
    logger.LogInformation("#                                           #");
    logger.LogInformation(" ###########################################");
    await host.RunAsync();
}
catch (Exception ex)
{
    logger.LogCritical(ex, "There was a problem starting the service");
    return;
}

