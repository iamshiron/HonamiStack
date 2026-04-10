using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public class HonamiBuilder<TUser, TKey, TDbContext>(WebApplicationBuilder builder)
    where TUser : HonamiUser<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext {
    internal readonly WebApplicationBuilder BuilderHandle = builder;
    public IServiceCollection Services => BuilderHandle.Services;

    private HonamiIdentityBuilder? _identityBuilder;
    private HonamiDbBuilder? _dbBuilder;

    public WebApplication Build() {
        _identityBuilder?.Process(this);
        _dbBuilder?.Process(this);

        return BuilderHandle.Build();
    }

    public HonamiIdentityBuilder AddIdentity() {
        _identityBuilder = new HonamiIdentityBuilder();
        return _identityBuilder;
    }

    public HonamiDbBuilder AddPostgres(string configPrefix) {
        _dbBuilder = new HonamiDbBuilder(configPrefix);
        return _dbBuilder;
    }
}
