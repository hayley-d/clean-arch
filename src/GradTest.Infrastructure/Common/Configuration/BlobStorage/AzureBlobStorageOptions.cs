using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GradTest.Infrastructure.Common.Configuration.BlobStorage;

public class AzureBlobStorageOptions
{
    public required string BlobStorageConnectionString { get; init; }
    public required string BlobEnvironment { get; init; }
}

public class AzureBlobStorageOptionsSetup : IConfigureOptions<AzureBlobStorageOptions>
{
    private const string Key = "AzureBlobStorage";
    
    private readonly IConfiguration _configuration;
    
    public AzureBlobStorageOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void Configure(AzureBlobStorageOptions options)
    {
        _configuration.GetRequiredSection(Key).Bind(options);
    }
}