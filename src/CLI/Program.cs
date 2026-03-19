using System.Globalization;
using Shiron.HonamiStack.CLI.Commands;
using Shiron.HonamiStack.CLI.Services;
using Shiron.HonamiStack.Core;
using Spectre.Console.Cli;

var logger = new Logger();
logger.Log("Test");
logger.Debug("Debug Test");
logger.LogMarkup("[blue]I'm blue[/]");
logger.Error("Error Test");

var app = new CommandApp();
app.Configure(c => {
    c.SetApplicationName(HonamiStack.AppName);
    c.SetApplicationVersion(HonamiStack.AppVersion);
    c.SetApplicationCulture(CultureInfo.CurrentCulture);

    c.AddCommand<AsyncInitCommand>("init");
});

logger.PrintHeader();
await app.RunAsync(args);
