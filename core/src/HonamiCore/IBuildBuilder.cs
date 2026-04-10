using Microsoft.EntityFrameworkCore;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public interface IBuildBuilder {
    void Process(HonamiBuilder builder);
}
