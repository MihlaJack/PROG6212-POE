using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RaceDayAPI.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient? _blobServiceClient;

    public bool IsConfigured { get; }

    public BlobStorageService(IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("AzureStorage");
        IsConfigured = !string.IsNullOrWhiteSpace(connectionString)
            && !connectionString.Contains("youraccount", StringComparison.OrdinalIgnoreCase)
            && !connectionString.Contains("yourkey", StringComparison.OrdinalIgnoreCase);

        if (IsConfigured)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }
    }

    private BlobServiceClient Client => _blobServiceClient
        ?? throw new InvalidOperationException("Azure Blob Storage is not configured.");

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string containerName)
    {
        var containerClient = Client.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true);

        return GetBlobUrl(fileName, containerName);
    }

    public async Task DeleteFileAsync(string fileName, string containerName)
    {
        var containerClient = Client.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
    }

    public string GetBlobUrl(string fileName, string containerName)
    {
        var containerClient = Client.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        return blobClient.Uri.ToString();
    }
}
