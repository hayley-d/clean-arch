using GradTest.Domain.BoundedContexts.Files.Entities;

namespace GradTest.Application.Common.Services;

public interface IBlobStorage
{
    Task<Result> UploadBlobAsync(
        Blob blob, 
        CancellationToken cancellationToken = default);
    
    Task<Result<Blob>> GetBlobContentAsync(
        Blob blob,
        CancellationToken cancellationToken = default);
    
    Task<Result> UpdateBlobAsync(
        Blob blob,
        CancellationToken cancellationToken = default);
    
    Task<Result> DeleteBlobAsync(
        Blob blob, 
        CancellationToken cancellationToken = default);
    
    Result<Uri> GenerateSasToken(Blob blob);
}