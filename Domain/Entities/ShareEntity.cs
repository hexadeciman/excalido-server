namespace Domain.Entities;

public class ShareEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int EntityId { get; set; }  // ID of the shared entity (Board, List, etc.)
    public int EntityTypeId { get; set; }  // Foreign key to EntityType
    public Guid? UserId { get; set; }
    public int PermissionId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    // Navigation properties
    public virtual  EntityType EntityType { get; set; } = null!;
    public virtual User? User { get; set; }
    public virtual Permission Permission { get; set; } = null!;
}
