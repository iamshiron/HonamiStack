using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public class HonamiDbBuilder(string configPrefix) {
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
                var host = Environment.GetEnvironmentVariable($"{configPrefix}_POSTGRES_HOST") ??
                    throw new InvalidOperationException($"POSTGRES_HOST environment variable is not set. Define {configPrefix}_POSTGRES_HOST");
                var port = Environment.GetEnvironmentVariable($"{configPrefix}_POSTGRES_PORT") ??
                    throw new InvalidOperationException($"POSTGRES_PORT environment variable is not set. Define {configPrefix}_POSTGRES_PORT");
                var database = Environment.GetEnvironmentVariable($"{configPrefix}_POSTGRES_DB") ??
                    throw new InvalidOperationException($"POSTGRES_DB environment variable is not set. Define {configPrefix}_POSTGRES_DB");
                var user = Environment.GetEnvironmentVariable($"{configPrefix}_POSTGRES_USER") ??
                    throw new InvalidOperationException($"POSTGRES_USER environment variable is not set. Define {configPrefix}_POSTGRES_USER");
                var password = Environment.GetEnvironmentVariable($"{configPrefix}_POSTGRES_PASSWORD") ??
                    throw new InvalidOperationException($"POSTGRES_PASSWORD environment variable is not set. Define {configPrefix}_POSTGRES_PASSWORD");

                _connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password}";
            }
        }

        handle.Services.AddDbContext<TDbContext>(o => {
            o.UseNpgsql(_connectionString);
        });
    }
}
