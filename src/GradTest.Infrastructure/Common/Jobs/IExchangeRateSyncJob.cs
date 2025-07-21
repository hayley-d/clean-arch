namespace GradTest.Shared.Jobs;

public interface IExchangeRateSyncJob
{
    Task SyncAndStoreAsync();
}