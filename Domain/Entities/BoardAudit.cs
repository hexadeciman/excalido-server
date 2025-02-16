namespace Domain.Entities;

public class BoardAudit
{
    public int Id { get; set; }
    public int BoardId { get; set; }
    public Guid UserId { get; set; }
    public string EntityType { get; set; } = null!;
    public int EntityId { get; set; }
    public ActionEnum Action { get; set; }
    public string? OldValue { get; set; }  // JSON stored as string
    public string? NewValue { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Board Board { get; set; } = null!;
    public User User { get; set; } = null!;
}

public enum ActionEnum
{
    Created,
    Deleted,
    Updated
}