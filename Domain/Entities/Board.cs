namespace Domain.Entities;

public class Board
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
    public bool IsArchived { get; set; } = false;

    // Navigation properties
    public User Owner { get; set; } = null!;
    public List<ListEntity> Lists { get; set; } = new();
    public List<BoardAudit> AuditLogs { get; set; } = new();
}