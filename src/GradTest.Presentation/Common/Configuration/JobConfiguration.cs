using GradTest.Application.Common.Services;
using GradTest.Shared.Jobs;
using Hangfire;

namespace GradTest.Presentation.Common.Configuration;

public static class JobConfiguration
{
    public static void SetupJobs(this IApplicationBuilder app)
    {
        var jobManager = app.ApplicationServices.GetRequiredService<IRecurringJobManager>();

        jobManager.AddOrUpdate<IExchangeRateSyncJob>(
            "sync-exchange-rate",
            job => job.SyncAndStoreAsync(),
            "0 */2 * * *"
        );
    }  
    
    public static WebApplicationBuilder SetupRucurringJobs(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();
        builder.Services.AddScoped<IExchangeRateSyncJob, ExchangeRateSyncJob>();
        
        return builder;
    } 
}