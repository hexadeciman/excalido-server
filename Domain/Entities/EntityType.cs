namespace Domain.Entities;

public class EntityType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation property (shared entities of this type)
    public virtual  List<ShareEntity> SharedEntities { get; set; } = new();
}