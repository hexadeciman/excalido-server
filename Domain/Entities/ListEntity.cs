namespace Domain.Entities;

public class ListEntity
{
    public int Id { get; set; }
    public int BoardId { get; set; }
    public string Name { get; set; } = null!;
    public bool Collapsed { get; set; } = false;
    public int XPosition { get; set; } = 0;
    public int YPosition { get; set; } = 0;
    public bool IsArchived { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Board Board { get; set; } = null!;
    public List<Todo> Todos { get; set; } = new();
}
