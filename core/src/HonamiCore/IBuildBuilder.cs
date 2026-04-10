using Microsoft.EntityFrameworkCore;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public interface IBuildBuilder {
    void Process<TUser, TKey, TDbContext>(HonamiBuilder<TUser, TKey, TDbContext> builder)
        where TUser : HonamiUser<TKey> where TKey : IEquatable<TKey> where TDbContext : DbContext;
}
