using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public class HonamiDbBuilder(string configPrefix) : IBuildBuilder {
    private string? _connectionString = null;

    public HonamiDbBuilder SetConnectionString(string connectionString) {
        _connectionString = connectionString;
        return this;
    }

    public void Process<TUser, TKey, TDbContext>(HonamiBuilder<TUser, TKey, TDbContext> builder)
        where TUser : HonamiUser<TKey>
        where TKey : IEquatable<TKey>
        where TDbContext : DbContext {
        var handle = builder.BuilderHandle;

        if (_connectionString == null) {
            _connectionString = handle.Configuration.GetConnectionString("Default")
                ?? handle.Configuration.GetSection($"{configPrefix}_ConnectionStrings")["Default"];

            if (_connectionString == null) {
                var host = handle.Configuration[$"{configPrefix}_POSTGRES_HOST"] ??
                    throw new InvalidOperationException($"POSTGRES_HOST variable is not set. Define {configPrefix}_POSTGRES_HOST");
                var port = handle.Configuration[$"{configPrefix}_POSTGRES_PORT"] ??
                    throw new InvalidOperationException($"POSTGRES_PORT variable is not set. Define {configPrefix}_POSTGRES_PORT");
                var database = handle.Configuration[$"{configPrefix}_POSTGRES_DB"] ??
                    throw new InvalidOperationException($"POSTGRES_DB variable is not set. Define {configPrefix}_POSTGRES_DB");
                var user = handle.Configuration[$"{configPrefix}_POSTGRES_USER"] ??
                    throw new InvalidOperationException($"POSTGRES_USER variable is not set. Define {configPrefix}_POSTGRES_USER");
                var password = handle.Configuration[$"{configPrefix}_POSTGRES_PASSWORD"] ??
                    throw new InvalidOperationException($"POSTGRES_PASSWORD variable is not set. Define {configPrefix}_POSTGRES_PASSWORD");

                _connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password}";
            }
        }

        handle.Services.AddDbContext<TDbContext>(o => {
            o.UseNpgsql(_connectionString);
        });
    }
}
