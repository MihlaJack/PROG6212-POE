namespace RaceDayAPI.Models;

public class Result
{
    public int ResultId { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public TimeSpan FinishTime { get; set; }
    public int FinishingPosition { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User User { get; set; } = null!;
    public Event Event { get; set; } = null!;
}
