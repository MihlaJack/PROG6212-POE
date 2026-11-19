namespace RaceDayAPI.Services;

public interface IBlobStorageService
{
    bool IsConfigured { get; }
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string containerName);
    Task DeleteFileAsync(string fileName, string containerName);
    string GetBlobUrl(string fileName, string containerName);
}
