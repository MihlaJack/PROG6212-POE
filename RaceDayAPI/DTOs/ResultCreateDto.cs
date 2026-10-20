namespace RaceDayAPI.DTOs;

public class ResultCreateDto
{
    public int UserId { get; set; }
    public int EventId { get; set; }
    public TimeSpan FinishTime { get; set; }
    public int FinishingPosition { get; set; }
}
