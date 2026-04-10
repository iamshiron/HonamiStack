using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public class HonamiReferenceBuilder(string title) {
    private string _route = "/api/scalar";
    private ScalarTheme _theme = ScalarTheme.Purple;
    private bool _darkMode = true;
    private string _title = title;

    public HonamiReferenceBuilder SetTitle(string title) {
        _title = title;
        return this;
    }
    public HonamiReferenceBuilder SetRoute(string route) {
        _route = route;
        return this;
    }
    public HonamiReferenceBuilder SetTheme(ScalarTheme theme) {
        _theme = theme;
        return this;
    }
    public HonamiReferenceBuilder DisableDarkMode(bool disable = true) {
        _darkMode = !disable;
        return this;
    }

    public void Process(WebApplication app) {
        app.MapScalarApiReference(_route, c => {
            c.Title = _title;
            c.Theme = _theme;
            c.DarkMode = _darkMode;
        });
    }
}
