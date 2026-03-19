using Spectre.Console.Cli;

namespace Shiron.HonamiStack.CLI.Commands;

public class AsyncInitCommand : AsyncCommand<AsyncInitCommand.Settings> {
    public sealed class Settings : CommandSettings {
    }

    public override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken) {
        Console.WriteLine($"Current Working Directory: {Directory.GetCurrentDirectory()}");
        return Task.FromResult(CliConstants.ExitCodes.Success);
    }
}
