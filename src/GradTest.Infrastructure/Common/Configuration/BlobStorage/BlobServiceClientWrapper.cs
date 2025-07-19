using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using GradTest.Infrastructure.Common.Services;

namespace GradTest.Infrastructure.Common.Configuration.BlobStorage;

public interface IBlobServiceClientWrapper
{
    BlobServiceClient GetClient();
}

public class BlobServiceClientWrapper: IBlobServiceClientWrapper
{
    private readonly BlobServiceClient _blobServiceClient;
    
    public BlobServiceClientWrapper(IOptions<AzureBlobStorageOptions> options)
    {
        _blobServiceClient = new BlobServiceClient(options.Value.BlobStorageConnectionString);
    }

    public BlobServiceClient GetClient() => _blobServiceClient;
}