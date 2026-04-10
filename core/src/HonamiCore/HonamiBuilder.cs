using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Shiron.HonamiCore;

public class HonamiBuilder(WebApplicationBuilder builder) {
    public WebApplication Build() {
        return builder.Build();
    }

    public IServiceCollection Services => builder.Services;
}
