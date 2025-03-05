namespace Domain.Entities;

public class Board
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
    public bool IsArchived { get; set; } = false;
    
    public int OrderIndex { get; set; }

    // Navigation properties
    public virtual User Owner { get; set; } = null!;
    public virtual List<ListEntity> Lists { get; set; } = new();
    public virtual List<BoardAudit> AuditLogs { get; set; } = new();
}