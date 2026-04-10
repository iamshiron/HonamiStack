using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public class HonamiApp {
    public static HonamiBuilder<TUser, TKey, TDbContext> CreateBuilder<TUser, TKey, TDbContext>(string[]? args = null)
        where TUser : HonamiUser<TKey>
        where TKey : IEquatable<TKey>
        where TDbContext : DbContext {
        return args == null
            ? new HonamiBuilder<TUser, TKey, TDbContext>(WebApplication.CreateBuilder())
            : new HonamiBuilder<TUser, TKey, TDbContext>(WebApplication.CreateBuilder(args));
    }
}
