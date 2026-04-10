using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public class HonamiApp(WebApplication app, IReadOnlyDictionary<Type, IAppBuilder[]> appBuilders) {
    public readonly WebApplication AppHandle = app;

    public static HonamiBuilder<TUser, TKey, TDbContext> CreateBuilder<TUser, TKey, TDbContext>(string name, string[]? args = null)
        where TUser : HonamiUser<TKey>
        where TKey : IEquatable<TKey>
        where TDbContext : DbContext {
        return args == null
            ? new HonamiBuilder<TUser, TKey, TDbContext>(name, WebApplication.CreateBuilder())
            : new HonamiBuilder<TUser, TKey, TDbContext>(name, WebApplication.CreateBuilder(args));
    }

    public void MapApiReference() {
        var builders = appBuilders[typeof(HonamiReferenceBuilder)];
        foreach (var builder in builders) {
            builder.Process(AppHandle);
        }
    }
}
