using System.Text.Json.Serialization;
using Shiron.HonamiStack.Core.Services;

namespace Shiron.HonamiStack.Core;

public class InitConfig() {
    public required string WorkspaceName { get; set; }
    public required IDBConfig? DBConfig { get; set; }
    public bool SeparateDBProject { get; set; } = true;
    public bool SeparateServiceProject { get; set; } = false;
    public bool UseEFCoreIdentity { get; set; } = false;
    public bool UseScalar { get; set; } = false;
}

public class HonamiConfig {
    public required string WorkspaceName { get; set; }
    public required Dictionary<Environment, IDBConfig> DBConfigs { get; set; }
    public required Dictionary<string, Project> Projects { get; set; }
}

public enum DBType {
    None,
    PostgreSQL,
    SQLite
}

public enum Environment {
    Development,
    Production
}

[JsonDerivedType(typeof(PostgresDBConfig), "postgres")]
[JsonDerivedType(typeof(SQLiteDBConfig), "sqlite")]
public interface IDBConfig {
}

public record PostgresDBConfig(string Host, uint Port, string Username, string Password) : IDBConfig {
}

public record SQLiteDBConfig(string File) : IDBConfig {
}

public abstract record Project(string Location, List<string> References) {
}

public record DotNetProject(string Location, List<string> References, DotNetProject.ProjectType Type) : Project(Location, References) {
    public enum ProjectType {
        ClassLib,
        Console,
        WebApi
    }
}

public record TSProject(string Location, List<string> References, TSProject.ProjectType Type)
    : Project(Location, References) {
    public enum ProjectType {
        ViteLibrary,
        ViteApplication
    }
}
