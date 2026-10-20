namespace RaceDayAPI.DTOs;

public class CategoryCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int EventId { get; set; }
}
