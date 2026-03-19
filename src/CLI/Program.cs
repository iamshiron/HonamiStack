using System.Globalization;
using Shiron.HonamiStack.CLI.Commands;
using Shiron.HonamiStack.Core;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(c => {
    c.SetApplicationName(HonamiStack.AppName);
    c.SetApplicationVersion(HonamiStack.AppVersion);
    c.SetApplicationCulture(CultureInfo.CurrentCulture);

    c.AddCommand<AsyncInitCommand>("init");
});

await app.RunAsync(args);
