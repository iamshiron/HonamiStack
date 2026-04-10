using System.Globalization;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(c => {
    c.SetApplicationName("hstack");
    c.SetApplicationVersion("0.0.0");
    c.SetApplicationCulture(CultureInfo.CurrentCulture);
});

await app.RunAsync(args);
