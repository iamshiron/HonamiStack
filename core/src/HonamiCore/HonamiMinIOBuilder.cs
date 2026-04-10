using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Shiron.HonamiCore.Types;

namespace Shiron.HonamiCore;

public class HonamiMinIOBuilder(string configPrefix) : IBuildBuilder {
    private S3AccessTriple? _accessTriple;
    private Endpoint? _endpoint;
    private bool _useSsl = true;

    public HonamiMinIOBuilder SetAuthentication(string accessKey, string secretKey, string region = "us-east-1") {
        return SetAuthentication(new S3AccessTriple(accessKey, secretKey, region));
    }
    public HonamiMinIOBuilder SetAuthentication(S3AccessTriple authTriple) {
        _accessTriple = authTriple;
        return this;
    }

    public HonamiMinIOBuilder SetEndpoint(Endpoint endpoint) {
        _endpoint = endpoint;
        return this;
    }
    public HonamiMinIOBuilder SetEndpoint(string host, int port) {
        return SetEndpoint(new Endpoint(host, port));
    }

    public HonamiMinIOBuilder UseSsl(bool useSsl) {
        _useSsl = useSsl;
        return this;
    }

    public void Process(HonamiBuilder builder) {
        var config = builder.BuilderHandle.Configuration;

        var endpoint = _endpoint ?? Endpoint.FromConfig(config, configPrefix);
        var access = _accessTriple ?? S3AccessTriple.FromConfig(config, configPrefix);

        builder.Services.AddSingleton<IMinioClient>(c =>
            new MinioClient()
                .WithEndpoint($"{endpoint.Host}:{endpoint.Port}")
                .WithCredentials(access.AccessKey, access.SecretKey)
                .WithRegion(access.Region)
                .WithSSL(_useSsl)
        );
    }
}
