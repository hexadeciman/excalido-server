namespace Application.DTOs.Board;

public class BoardDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
    public bool IsArchived { get; set; }
}