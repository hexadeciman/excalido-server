namespace Application.DTOs;

public class TodoDTO
{
    public int Id { get; set; }
    public int ListId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "unchecked";
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}