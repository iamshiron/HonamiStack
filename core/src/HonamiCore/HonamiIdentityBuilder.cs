using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public class HonamiIdentityBuilder : IBuildBuilder {
    private bool _configureCookie = false;
    private bool _requireUniqueEmail = false;
    private bool _disablePasswordRules = false;

    public HonamiIdentityBuilder ConfigureCookie(bool b = true) {
        _configureCookie = b;
        return this;
    }
    public HonamiIdentityBuilder RequireUniqueEmail(bool b = true) {
        _requireUniqueEmail = b;
        return this;
    }
    public HonamiIdentityBuilder DisablePasswordRules(bool b = false) {
        _disablePasswordRules = b;
        return this;
    }


    public void Process<TUser, TKey, TDbContext>(HonamiBuilder<TUser, TKey, TDbContext> builder)
        where TUser : HonamiUser<TKey>
        where TKey : IEquatable<TKey>
        where TDbContext : DbContext {
        var handle = builder.BuilderHandle;

        if (_configureCookie) {
            handle.Services.ConfigureApplicationCookie(c => {
                c.Events.OnRedirectToLogin = context => {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
                c.Events.OnRedirectToAccessDenied = context => {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };
            });
        }

        handle.Services.AddIdentity<TUser, IdentityRole<TKey>>(c => {
            if (_disablePasswordRules) {
                c.Password.RequireDigit = false;
                c.Password.RequiredLength = 4;
                c.Password.RequireNonAlphanumeric = false;
                c.Password.RequireUppercase = false;
                c.Password.RequireLowercase = false;
            }
            c.User.RequireUniqueEmail = _requireUniqueEmail;
        }).AddEntityFrameworkStores<TDbContext>().AddDefaultTokenProviders();
    }
}
