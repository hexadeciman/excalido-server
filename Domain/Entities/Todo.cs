namespace Domain.Entities;

public class Todo
{
    public int Id { get; set; }
    public int? ParentTodoId { get; set; }
    public int ListId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public TodoStatusEnum Status { get; set; } = TodoStatusEnum.Unchecked;
    public int OrderIndex { get; set; } = 0;
    public bool IsArchived { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ListEntity List { get; set; }
    public virtual Todo? ParentTodo { get; set; }
    public virtual List<Todo> SubTodos { get; set; } = new();
}

public enum TodoStatusEnum
{
    Unchecked,
    Checked,
    PartlyChecked
}
