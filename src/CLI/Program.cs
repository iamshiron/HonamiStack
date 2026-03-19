using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Shiron.HonamiStack.CLI.Commands;
using Shiron.HonamiStack.CLI.DI;
using Shiron.HonamiStack.CLI.Services;
using Shiron.HonamiStack.Core;
using Shiron.HonamiStack.Core.Services;
using Spectre.Console.Cli;

var logger = new Logger();

var services = new ServiceCollection();
services.AddSingleton<ILogger>(logger);

var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);
app.Configure(c => {
    c.SetApplicationName(HonamiStack.AppName);
    c.SetApplicationVersion(HonamiStack.AppVersion);
    c.SetApplicationCulture(CultureInfo.CurrentCulture);

    c.AddCommand<AsyncInitCommand>("init");
});

logger.PrintHeader();
await app.RunAsync(args);
