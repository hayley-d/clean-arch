using GradTest.Domain.BoundedContexts.ExchangeRates.Repositories;
using GradTest.Domain.Common.Entities;
using GradTest.Infrastructure.Common.Repository;
using GradTest.Infrastructure.Persistence;

namespace GradTest.Infrastructure.BoundedContexts.ExchangeRates;

public class ExchangeRatesRepository: BaseRepository, IExchangeRateRepository 
{
    private readonly ApplicationDbContext _dbContext;
    
    public ExchangeRatesRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<decimal?> GetLatestExchangeRate(CancellationToken cancellationToken = default)
    {
        var latestRate =  _dbContext.ExchangeRates
            .OrderByDescending(rate => rate.Date)
            .Select(rate => rate.ZAR).FirstOrDefault();
        
        return latestRate;
    }
    
    
}