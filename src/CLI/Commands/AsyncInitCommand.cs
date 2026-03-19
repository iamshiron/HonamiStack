using Shiron.HonamiStack.Core.Services;
using Spectre.Console.Cli;

namespace Shiron.HonamiStack.CLI.Commands;

public class AsyncInitCommand(ILogger logger) : AsyncCommand<AsyncInitCommand.Settings> {
    private readonly ILogger _logger = logger;

    public sealed class Settings : CommandSettings {
    }

    public override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken) {
        _logger.Log($"Current Working Directory: {Directory.GetCurrentDirectory()}");
        return Task.FromResult(CliConstants.ExitCodes.Success);
    }
}
