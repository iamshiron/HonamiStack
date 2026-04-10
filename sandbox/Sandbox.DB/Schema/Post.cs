using System.ComponentModel.DataAnnotations;

namespace Shiron.HonamiCore.Sandbox.DB.Schema;

public class Post {
    public Guid ID { get; set; } = Guid.CreateVersion7();

    [MaxLength(255)] public required string Title { get; set; }
    [MaxLength(4095)] public required string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public required Guid UserID { get; set; }
    public required User User { get; set; }
}
