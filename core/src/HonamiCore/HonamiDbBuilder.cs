using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public class HonamiDbBuilder<TContext>(string configPrefix) : IBuildBuilder where TContext : DbContext {
    private string? _connectionString;

    public HonamiDbBuilder<TContext> SetConnectionString(string connectionString) {
        _connectionString = connectionString;
        return this;
    }

    public void Process(HonamiBuilder builder) {
        var config = builder.BuilderHandle.Configuration;
        _connectionString ??= config.GetConnectionString("Default")
            ?? config.GetSection($"{configPrefix}_ConnectionStrings")["Default"]
            ?? BuildFromComponents(config);

        builder.BuilderHandle.Services.AddDbContext<TContext>(o => {
            o.UseNpgsql(_connectionString);
        });
    }

    private string BuildFromComponents(IConfiguration config) {
        var builder = new NpgsqlConnectionStringBuilder {
            Host = GetRequired(config, "HOST"),
            Port = int.Parse(GetRequired(config, "PORT")),
            Database = GetRequired(config, "DB"),
            Username = GetRequired(config, "USER"),
            Password = GetRequired(config, "PASSWORD")
        };
        return builder.ToString();
    }

    private string GetRequired(IConfiguration config, string key) {
        return config[$"{configPrefix}_POSTGRES_{key}"] ?? throw new InvalidOperationException($"{configPrefix}_POSTGRES_{key} is not set.");
    }
}
