using Microsoft.AspNetCore.Builder;

namespace Shiron.HonamiCore;

public class HonamiApp {
    public static HonamiBuilder CreateBuilder(string[]? args = null) {
        return args == null ? new HonamiBuilder(WebApplication.CreateBuilder()) : new HonamiBuilder(WebApplication.CreateBuilder(args));
    }
}
