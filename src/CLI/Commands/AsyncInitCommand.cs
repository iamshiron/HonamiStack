using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Shiron.HonamiStack.Core;
using Shiron.HonamiStack.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shiron.HonamiStack.CLI.Commands;

[UsedImplicitly]
public class AsyncInitCommand(ILogger logger) : AsyncCommand<AsyncInitCommand.Settings> {
    private readonly ILogger _logger = logger;
    private readonly JsonSerializerOptions _jsonOptions = new() {
        IndentSize = 4,
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    [UsedImplicitly]
    public sealed class Settings : CommandSettings {
    }

    public async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken) {
        var config = new Config("Test", [
            new FrontendModule("web", "apps/web"),
            new BackendModule("core", "backend/core")
        ]);

        await File.WriteAllTextAsync("honami.config.json", JsonSerializer.Serialize(config, _jsonOptions), cancellationToken);
        return 0;
    }
}
