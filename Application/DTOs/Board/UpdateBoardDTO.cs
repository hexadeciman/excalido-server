namespace Application.DTOs.Board;

public class UpdateBoardDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsArchived { get; set; }
}