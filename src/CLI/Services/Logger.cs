using Shiron.HonamiStack.Core;
using Shiron.HonamiStack.Core.Services;
using Spectre.Console;

namespace Shiron.HonamiStack.CLI.Services;

public class Logger : ILogger {
    public void PrintHeader() {
        var figlet = new FigletText("HStack") {
            Color = Color.Fuchsia,
            Justification = Justify.Left
        };

        AnsiConsole.Write(figlet);
    }

    public void Log(string message) {
        AnsiConsole.Write(new Text("[", new Style(Color.Grey)));
        AnsiConsole.Write(new Text("Honami", new Style(Color.Fuchsia)));
        AnsiConsole.Write(new Text("] ", new Style(Color.Grey)));

        AnsiConsole.WriteLine(message);
    }
    public void LogMarkup(string message, bool skipPrefix = false) {
        if (!skipPrefix) {
            AnsiConsole.Write(new Text("[", new Style(Color.Grey)));
            AnsiConsole.Write(new Text("Honami", new Style(Color.Fuchsia)));
            AnsiConsole.Write(new Text("] ", new Style(Color.Grey)));
        }

        try {
            AnsiConsole.MarkupLine(message);
        } catch (Exception e) {
            AnsiConsole.WriteLine(message);
        }
    }
    public void Debug(string message) {
        AnsiConsole.Write(new Text("[", new Style(Color.Grey)));
        AnsiConsole.Write(new Text("Honami", new Style(Color.Fuchsia)));
        AnsiConsole.Write(new Text("/Debug] ", new Style(Color.Grey)));

        AnsiConsole.WriteLine(message);
    }

    public void Error(string message) {
        AnsiConsole.Write(new Text("[", new Style(Color.Red)));
        AnsiConsole.Write(new Text("Honami", new Style(Color.Fuchsia)));
        AnsiConsole.Write(new Text("/Error] ", new Style(Color.Red)));

        AnsiConsole.WriteLine(message);
    }
    public void Exception(Exception exception) {
        AnsiConsole.WriteException(exception);
    }
}

public static class LogExtensions {
    public static void LogInitConfig(this ILogger logger, InitConfig config) {
        logger.LogMarkup("[grey]---[/] [fuchsia]HonamiStack Project Configuration[/] [grey]---[/]", true);
        logger.LogMarkup($"[blue]Name[/]: {config.WorkspaceName}", true);
        logger.LogMarkup($"[blue]Use EF Core Identity[/]: {config.UseEFCoreIdentity}", true);
        logger.LogMarkup($"[blue]Use Scalar[/]: {config.UseScalar}", true);
        logger.LogMarkup($"[blue]Separate DB Schema Project [/]: {config.SeparateDBProject}", true);
        logger.LogMarkup($"[blue]Separate Service Project [/]: {config.SeparateServiceProject}", true);
    }
}
