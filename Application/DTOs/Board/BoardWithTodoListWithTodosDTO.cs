using Application.DTOs.TodoList;

namespace Application.DTOs.Board;

public class BoardWithTodoListWithTodosDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<TodoListWithTodosDTO> TodoLists { get; set; }
}