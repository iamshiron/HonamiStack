using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public static class HonamiApp {
    public static HonamiBuilder<TUser, TKey, TDbContext> CreateBuilder<TUser, TKey, TDbContext>(string name, string[]? args = null)
        where TUser : HonamiUser<TKey>
        where TKey : IEquatable<TKey>
        where TDbContext : DbContext {
        return args == null
            ? new HonamiBuilder<TUser, TKey, TDbContext>(name, WebApplication.CreateBuilder())
            : new HonamiBuilder<TUser, TKey, TDbContext>(name, WebApplication.CreateBuilder(args));
    }
}
