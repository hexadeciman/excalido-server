namespace Domain.Entities;

public class UserRole
{
    public Guid UserId { get; set; }
    public int PermissionId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User User { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}