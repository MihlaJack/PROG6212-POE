using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.Services;

namespace RaceDayAPI.Controllers;

/// <summary>
/// Handles file uploads for event banners and profile pictures
/// </summary>
[ApiController]
[Route("api/files")]
public class FileUploadController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;

    public FileUploadController(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    /// <summary>
    /// Uploads an event banner image
    /// </summary>
    /// <returns>URL of the uploaded image</returns>
    [HttpPost("upload/event-banner")]
    public async Task<IActionResult> UploadEventBanner(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded" });
        }

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        string imageUrl = await _blobStorageService.UploadFileAsync(stream, fileName, "event-banners");

        return Ok(new { imageUrl });
    }

    /// <summary>
    /// Uploads a profile picture
    /// </summary>
    /// <returns>URL of the uploaded image</returns>
    [HttpPost("upload/profile-picture")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded" });
        }

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        string imageUrl = await _blobStorageService.UploadFileAsync(stream, fileName, "profile-pictures");

        return Ok(new { imageUrl });
    }
}
