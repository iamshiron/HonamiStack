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

public enum DBType {
    None,
    PostgreSQL,
    SQLite
}

public interface IDBConfig {
}

public record PostgresDBConfig(string Host, uint Port, string Username, string Password) : IDBConfig {
}

public record SQLiteDBConfig(string File) : IDBConfig {
}
