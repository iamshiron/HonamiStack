using Microsoft.AspNetCore.Builder;

namespace Shiron.HonamiCore;

public interface IAppBuilder {
    void Process(WebApplication app);
}
