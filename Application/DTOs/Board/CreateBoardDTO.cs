namespace Application.DTOs.Board;

public class CreateBoardDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
}
