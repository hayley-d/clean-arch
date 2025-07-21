using GradTest.Application.Common.Services;
using GradTest.Infrastructure.Common.Errors;
using GradTest.Infrastructure.Persistence;
using GradTest.Shared.Monads;
using Microsoft.Extensions.Logging;

namespace GradTest.Shared.Jobs;

public class ExchangeRateSyncJob: IExchangeRateSyncJob
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<ExchangeRateSyncJob> _logger;
    private readonly ApplicationDbContext _context;

    public ExchangeRateSyncJob(IExchangeRateService exchangeRateService, ILogger<ExchangeRateSyncJob> logger,
        ApplicationDbContext context)
    {
        _exchangeRateService = exchangeRateService;
        _logger = logger;
        _context = context;
    }

    public async Task SyncAndStoreAsync()
    {
        _logger.LogInformation($"Start syncing exchange rates at {DateTime.Now}");

        try
        {
            var rate = await _exchangeRateService.GetExchangeRateAsync();
            
            if (rate == null)
            {
                _logger.LogError("Exchange rate could not be retrieved.");
                return;
            }

         
            if (rate is not null)
            {
                _context.ExchangeRates.Add(rate);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Exchange rate sync complete");
            }
            else
            {
                _logger.LogError("Exchange rate not found, cannot sync.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unexpected error during exchange rate sync: {ex.Message}");
        }
      
        _logger.LogInformation($"Exchange rate sync job completed at {DateTime.Now}");
    } 
}