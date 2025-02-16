namespace Api.Models.Requests;

public class CreateTodo
{
    public int ListId { get; set; }  // Foreign Key
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
