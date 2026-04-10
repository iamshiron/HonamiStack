using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public class HonamiApp(WebApplication app, IReadOnlyDictionary<Type, IAppBuilder[]> appBuilders) {
    public readonly WebApplication AppHandle = app;

    public static HonamiBuilder CreateBuilder(string name, string[]? args = null) {
        return args == null
            ? new HonamiBuilder(name, WebApplication.CreateBuilder())
            : new HonamiBuilder(name, WebApplication.CreateBuilder(args));
    }

    public void MapApiReference() {
        var builders = appBuilders[typeof(HonamiReferenceBuilder)];
        foreach (var builder in builders) {
            builder.Process(AppHandle);
        }
    }
}
