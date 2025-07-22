using GradTest.Domain.Common.Contracts;
using GradTest.Domain.Common.Entities;

namespace GradTest.Domain.BoundedContexts.ExchangeRates.Repositories;

public interface IExchangeRateRepository: IRepository
{
    public Task<decimal?> GetLatestExchangeRate(CancellationToken cancellationToken = default);
}