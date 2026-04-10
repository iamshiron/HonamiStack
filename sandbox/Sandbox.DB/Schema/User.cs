using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore.Sandbox.DB.Schema;

public class User : HonamiUser<Guid> {
    public IList<Post> Posts { get; set; } = [];
}
