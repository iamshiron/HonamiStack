using Microsoft.EntityFrameworkCore;
using Shiron.HonamiCore.EFCore;
using Shiron.HonamiCore.EFCore.Entities;
using Shiron.HonamiCore.Sandbox.DB.Schema;

namespace Shiron.HonamiCore.Sandbox.DB;

public class SandboxDb(DbContextOptions options) : HonamiIdentityDb<Guid, User>(options) {
    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        builder.Entity<Post>(entity => {
            entity.HasKey(e => e.ID);

            entity.HasOne(e => e.User)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<User>(entity => {
            entity.HasMany(e => e.Posts)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
