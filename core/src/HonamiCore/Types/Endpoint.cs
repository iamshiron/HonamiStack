using Microsoft.Extensions.Configuration;

namespace Shiron.HonamiCore.Types;

public readonly record struct Endpoint(string Host, int Port) {
    public static Endpoint FromConfig(IConfiguration config, string prefix) {
        return new Endpoint(
            GetRequired(config, prefix, "HOST"),
            int.Parse(GetRequired(config, prefix, "PORT"))
        );
    }

    private static string GetRequired(IConfiguration config, string prefix, string key) {
        return config[$"{prefix}_{key}"] ?? throw new InvalidOperationException($"{prefix}_{key} is not set.");
    }
}
