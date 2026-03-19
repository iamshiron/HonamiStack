using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Shiron.HonamiStack.CLI.Services;
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
        var workspaceName = await AnsiConsole.PromptAsync(new TextPrompt<string>("What is the name of your project?")
                .DefaultValue("new-honami-project").Validate(input => {
                    if (!input.ToLower().Equals(input)) {
                        return ValidationResult.Error("Project names don't support uppercase characters");
                    }
                    if (input.Trim().Length == 0 || input.Trim() == string.Empty) {
                        return ValidationResult.Error("Project names can't be empty");
                    }
                    if (input.Contains(' ')) {
                        return ValidationResult.Error("Project names can't contain spaces");
                    }

                    return ValidationResult.Success();
                }), cancellationToken
        );
        var projectPath = Path.Combine(Directory.GetCurrentDirectory(), workspaceName);

        if (Directory.Exists(projectPath)) {
            _logger.Error("Directory already exists");
            var overwrite = await AnsiConsole.ConfirmAsync("Do you want to overrite the existing directory?", false, cancellationToken);
            if (overwrite) {
                Directory.Delete(projectPath, true);
            } else {
                return CliConstants.ExitCodes.Aborted;
            }
        }

        IDBConfig? dbConfig = null;

        var dbType = await AnsiConsole.PromptAsync(
            new SelectionPrompt<DBType>()
                .Title("Select your [fuchsia]Database[/] type:")
                .AddChoices(DBType.PostgreSQL, DBType.SQLite, DBType.None),
            cancellationToken
        );

        var useSeparateDbProject = false;
        var useIdentity = false;
        if (dbType != DBType.None) {
            useIdentity =
                await AnsiConsole.ConfirmAsync(
                    "Do you want to add EFCore Identity to your project?", false, cancellationToken);
            useSeparateDbProject =
                await AnsiConsole.ConfirmAsync(
                    "Do you want to use a separate project containing your schema definitions?", false, cancellationToken);

            switch (dbType) {
                case DBType.PostgreSQL:
                    dbConfig = new PostgresDBConfig("127.0.0.1", 5432, "honami_dev", "honami_dev");
                    break;
                case DBType.SQLite:
                    dbConfig = new SQLiteDBConfig("./honami.db");
                    break;
                default:
                    throw new Exception("Unknown database type");
            }
        }

        var useSeparateServiceProject =
            await AnsiConsole.ConfirmAsync("Do you want to use a separate project for your services?", false, cancellationToken);
        var useScalar = await AnsiConsole.ConfirmAsync("Do you want to add scalar for your API documentation?", false, cancellationToken);

        var config = new InitConfig {
            WorkspaceName = workspaceName,
            DBConfig = dbConfig,
            SeparateDBProject = useSeparateDbProject,
            SeparateServiceProject = useSeparateServiceProject,
            UseEFCoreIdentity = useIdentity,
            UseScalar = useScalar
        };

        logger.LogInitConfig(config);
        return 0;
    }
}
