using System.ComponentModel.DataAnnotations;

namespace RaceDayWeb.Models;

// Request Models
public class LoginRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; } = string.Empty;
}

public class EventCreateRequest
{
    [Required(ErrorMessage = "Event name is required")]
    [StringLength(100, ErrorMessage = "Event name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Event date is required")]
    public DateTime EventDate { get; set; }

    [Required(ErrorMessage = "Location is required")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Distance is required")]
    [Range(0.1, 500, ErrorMessage = "Distance must be between 0.1 and 500 km")]
    public decimal Distance { get; set; }

    [Required(ErrorMessage = "Event type is required")]
    public string EventType { get; set; } = string.Empty;

    public string? BannerImageUrl { get; set; }
}

public class EventUpdateRequest
{
    [Required(ErrorMessage = "Event name is required")]
    [StringLength(100, ErrorMessage = "Event name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Event date is required")]
    public DateTime EventDate { get; set; }

    [Required(ErrorMessage = "Location is required")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Distance is required")]
    [Range(0.1, 500, ErrorMessage = "Distance must be between 0.1 and 500 km")]
    public decimal Distance { get; set; }

    [Required(ErrorMessage = "Event type is required")]
    public string EventType { get; set; } = string.Empty;

    public string? BannerImageUrl { get; set; }
}

public class CategoryCreateRequest
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Event ID is required")]
    public int EventId { get; set; }
}

public class EnrolmentRequest
{
    [Required(ErrorMessage = "Event ID is required")]
    public int EventId { get; set; }

    [Required(ErrorMessage = "Category ID is required")]
    public int CategoryId { get; set; }
}

public class ResultCreateRequest
{
    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Event ID is required")]
    public int EventId { get; set; }

    [Required(ErrorMessage = "Finish time is required")]
    public TimeSpan FinishTime { get; set; }

    [Required(ErrorMessage = "Finishing position is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Position must be at least 1")]
    public int FinishingPosition { get; set; }
}

public class UserProfileUpdateRequest
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    public string? ProfilePictureUrl { get; set; }
}

// Response Models
public class LoginResponse
{
    public string Message { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Role { get; set; } = string.Empty;
}

public class EventDto
{
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal Distance { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string? BannerImageUrl { get; set; }
    public string OrganiserName { get; set; } = string.Empty;
    public List<CategorySummaryDto> Categories { get; set; } = new();
}

public class CategoryDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int EventId { get; set; }
}

public class CategorySummaryDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class EnrolmentDto
{
    public int EnrolmentId { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
    public string? ParticipantName { get; set; }
    public string? ParticipantEmail { get; set; }
}

public class ResultDto
{
    public int ResultId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public TimeSpan FinishTime { get; set; }
    public int FinishingPosition { get; set; }
    public DateTime RecordedAt { get; set; }
}

public class UserProfileDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UploadResponse
{
    public string ImageUrl { get; set; } = string.Empty;
}
