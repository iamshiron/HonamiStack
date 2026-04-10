using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public class HonamiBuilder<TUser, TKey, TDbContext>(string appName, WebApplicationBuilder builder)
    where TUser : HonamiUser<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext {
    internal readonly WebApplicationBuilder BuilderHandle = builder;
    public IServiceCollection Services => BuilderHandle.Services;

    // Build-phase builders
    private HonamiIdentityBuilder? _identityBuilder;
    private HonamiDbBuilder? _dbBuilder;

    // App-phase builders
    private readonly Dictionary<Type, IAppBuilder[]> _appBuilders = [];

    public HonamiApp Build() {
        _dbBuilder?.Process(this);
        _identityBuilder?.Process(this);

        return new HonamiApp(BuilderHandle.Build(), _appBuilders);
    }

    public HonamiIdentityBuilder AddIdentity() {
        _identityBuilder = new HonamiIdentityBuilder();
        return _identityBuilder;
    }

    public HonamiDbBuilder AddPostgres(string configPrefix) {
        _dbBuilder = new HonamiDbBuilder(configPrefix);
        return _dbBuilder;
    }

    public HonamiReferenceBuilder AddApiReference(string? title = null) {
        var builder = new HonamiReferenceBuilder(title ?? appName);
        _appBuilders.Add(typeof(HonamiReferenceBuilder), [builder]);

        return builder;
    }
}
