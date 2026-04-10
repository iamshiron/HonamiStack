using Microsoft.Extensions.Configuration;

namespace Shiron.HonamiCore.Types;

public readonly record struct S3AccessTriple(string AccessKey, string SecretKey, string Region) {
    public static S3AccessTriple FromConfig(IConfiguration config, string prefix) {
        return new S3AccessTriple(
            GetRequired(config, prefix, "ACCESS_KEY"),
            GetRequired(config, prefix, "SECRET_KEY"),
            GetRequired(config, prefix, "REGION")
        );
    }

    private static string GetRequired(IConfiguration config, string prefix, string key) {
        return config[$"{prefix}_{key}"] ?? throw new InvalidOperationException($"{prefix}_{key} is not set.");
    }
}
