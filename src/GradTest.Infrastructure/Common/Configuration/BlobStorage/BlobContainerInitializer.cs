using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace GradTest.Infrastructure.Common.Configuration.BlobStorage;

public interface IBlobContainerInitializer
{
    Task InitializeContainerAsync(CancellationToken cancellationToken = default);
}

public class BlobContainerInitializer: IBlobContainerInitializer
{
    private readonly string _connectionString;
    private readonly string _environment;
    
    public BlobContainerInitializer(IOptions<AzureBlobStorageOptions> options)
    {
        _connectionString = options.Value.BlobStorageConnectionString;
        _environment = options.Value.BlobEnvironment;
    }
    
    public async Task InitializeContainerAsync(CancellationToken cancellationToken = default)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        string[] blobContainers = [];
        
        foreach (var container in blobContainers)
        {
            await blobServiceClient
                .GetBlobContainerClient($"{_environment}-{container}")
                .CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        }
    }
}