namespace Application.DTOs;

public class CreateTodoDTO
{ 
    public int ListId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public int? Index { get; set; }
}