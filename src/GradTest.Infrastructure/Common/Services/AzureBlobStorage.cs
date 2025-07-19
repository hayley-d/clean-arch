using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;
using GradTest.Application.Common.Services;
using GradTest.Domain.BoundedContexts.Files.Entities;
using GradTest.Infrastructure.Common.Configuration.BlobStorage;
using GradTest.Infrastructure.Common.Errors;

namespace GradTest.Infrastructure.Common.Services;

public class AzureBlobStorage: IBlobStorage
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<AzureBlobStorage> _logger;

    public AzureBlobStorage(
        IBlobServiceClientWrapper blobServiceClientWrapper, 
        ILogger<AzureBlobStorage> logger)
    {
        throw new NotImplementedException("AzureBlobStorage needs an implementation for <YOUR_CONTAINER_NAME>");
        
        _blobServiceClient = blobServiceClientWrapper.GetClient();
        _logger = logger;
    }

    public async Task<Result> UploadBlobAsync(
        Blob blob, 
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("<YOUR_CONTAINER_NAME>");
        var blobClient = containerClient.GetBlobClient(blob.StorageName);
        
        using var ms = new MemoryStream(blob.Content);
        ms.Position = 0;
        
        try
        {
            await blobClient.UploadAsync(ms, cancellationToken);
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to upload blob {@BlobId}", blob.Id);
            return Result.Error(BlobStorageError.Create());
        }
    }
    
    public async Task<Result<Blob>> GetBlobContentAsync(
        Blob blob,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("<YOUR_CONTAINER_NAME>");
        var blobClient = containerClient.GetBlobClient(blob.StorageName);
        using var ms = new MemoryStream();
        
        try
        {
            await blobClient.DownloadToAsync(ms, cancellationToken);
            blob.InitializeContent(ms);
            
            return Result<Blob>.Success(blob);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to read blob {@BlobId}", blob.Id);
            return Result<Blob>.Error(BlobStorageError.Create());
        }
    }

    public async Task<Result> UpdateBlobAsync(
        Blob blob, 
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("<YOUR_CONTAINER_NAME>");
        var blobClient = containerClient.GetBlobClient(blob.StorageName);
        
        using var ms = new MemoryStream(blob.Content);
        ms.Position = 0;
        
        try
        {
            // NOTE: if you want to update a blob from one container to another,
            // you will need a different implementation than this
            
            await blobClient.UploadAsync(ms, overwrite: true, cancellationToken);
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete blob {@BlobId}", blob.Id);
            return Result.Error(BlobStorageError.Create());
        }
    }

    public async Task<Result> DeleteBlobAsync(
        Blob blob,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("<YOUR_CONTAINER_NAME>");
        var blobClient = containerClient.GetBlobClient(blob.StorageName);
        
        try
        {
            await blobClient.DeleteIfExistsAsync(
                DeleteSnapshotsOption.IncludeSnapshots,
                cancellationToken: cancellationToken);
            
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete blob {@BlobId}", blob.Id);
            return Result.Error(BlobStorageError.Create());
        }
    }
    
    public Result<Uri> GenerateSasToken(Blob blob)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("<YOUR_CONTAINER_NAME>");
        var blobClient = containerClient.GetBlobClient(blob.StorageName);

        if (!blobClient.CanGenerateSasUri)
        {
            _logger.LogError("Unable to generate an SAS token for blob {@BlobId}", blob.Id);
            return Result<Uri>.Error(BlobStorageError.Create());
        }
            
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerClient.Name,
            BlobName = blobClient.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
        };
            
        sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);

        try
        {
            var sasUri = blobClient.GenerateSasUri(sasBuilder);
            return Result<Uri>.Success(sasUri);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed generate an SAS token for blob {@BlobId}", blob.Id);
            return Result<Uri>.Error(BlobStorageError.Create());
        }
    }
}