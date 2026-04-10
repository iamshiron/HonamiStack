using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore.EFCore;

public class HonamiIdentityDb<TKey, TUser>(DbContextOptions options) : IdentityDbContext<TUser, IdentityRole<TKey>, TKey>(options)
    where TKey : IEquatable<TKey>
    where TUser : HonamiUser<TKey> {
    public override DbSet<TUser> Users => Set<TUser>();

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        builder.Entity<HonamiUser<TKey>>().ToTable("Users");
        builder.Entity<IdentityRole<TKey>>().ToTable("Roles");
        builder.Entity<IdentityUserRole<TKey>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<TKey>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<TKey>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<TKey>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<TKey>>().ToTable("UserTokens");
    }
}
