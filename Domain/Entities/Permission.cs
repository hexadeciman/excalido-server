namespace Domain.Entities;

public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    // Navigation property
    public virtual  List<UserRole> UserRoles { get; set; } = new();
}
