using Microsoft.AspNetCore.Identity;

namespace Shiron.HonamiCore.EFCore.Entities;

public class HonamiUser<T> : IdentityUser<T> where T : IEquatable<T> {
}
