namespace Application.DTOs.TodoList;

public class TodoListWithTodosDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BoardId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public List<TodoDTO> Todos { get; set; }
}