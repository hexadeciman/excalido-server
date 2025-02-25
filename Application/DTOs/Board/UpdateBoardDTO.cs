namespace Application.DTOs.Board;

public class UpdateBoardDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsArchived { get; set; }
}