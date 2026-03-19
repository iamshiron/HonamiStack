using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shiron.HonamiStack.Core;

public class WorkspaceInitializer(InitConfig config) {
    private readonly JsonSerializerOptions _jsonOptions = new() {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        IndentSize = 4
    };

    public async Task Initialize() {
        var cfg = new HonamiConfig {
            WorkspaceName = config.WorkspaceName,
            DBConfigs = [],
            Projects = []
        };

        if (config.DBConfig != null) {
            cfg.DBConfigs[Environment.Development] = config.DBConfig;
            cfg.DBConfigs[Environment.Production] = config.DBConfig;
        }

        if (config.SeparateDBProject) {
            cfg.Projects["db"] = new DotNetProject("backend/db", [], DotNetProject.ProjectType.ClassLib);
        }
        if (config.SeparateServiceProject) {
            cfg.Projects["service"] = new DotNetProject("backend/service", [], DotNetProject.ProjectType.ClassLib);
        }

        cfg.Projects["server"] = new DotNetProject("backend/server", [], DotNetProject.ProjectType.WebApi);
        cfg.Projects["web"] = new TSProject("frontned/web", [], TSProject.ProjectType.ViteApplication);

        await File.WriteAllTextAsync("honami.config.json", JsonSerializer.Serialize(cfg, _jsonOptions));
    }
}
