using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public class HonamiBuilder(string appName, WebApplicationBuilder builder) {
    internal readonly WebApplicationBuilder BuilderHandle = builder;
    public IServiceCollection Services => BuilderHandle.Services;

    // Build-phase builders
    private readonly List<IBuildBuilder> _builders = [];

    // App-phase builders
    private readonly Dictionary<Type, IAppBuilder[]> _appBuilders = [];

    public HonamiApp Build() {
        foreach (var builder in _builders) {
            builder.Process(this);
        }

        return new HonamiApp(BuilderHandle.Build(), _appBuilders);
    }

    public HonamiIdentityBuilder<TUser, TKey, TDbContext> AddIdentity<TUser, TKey, TDbContext>()
        where TUser : HonamiUser<TKey> where TKey : IEquatable<TKey> where TDbContext : DbContext {
        var builder = new HonamiIdentityBuilder<TUser, TKey, TDbContext>();
        _builders.Add(builder);
        return builder;
    }

    public HonamiDbBuilder<TContext> AddPostgres<TContext>(string configPrefix) where TContext : DbContext {
        var builder = new HonamiDbBuilder<TContext>(configPrefix);
        _builders.Add(builder);
        return builder;
    }

    public HonamiReferenceBuilder AddApiReference(string? title = null) {
        var builder = new HonamiReferenceBuilder(title ?? appName);
        _appBuilders.Add(typeof(HonamiReferenceBuilder), [builder]);

        return builder;
    }
}
