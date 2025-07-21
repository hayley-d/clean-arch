using GradTest.Domain.Common.Entities;

namespace GradTest.Application.Common.Services;

public interface IExchangeRateService
{
    Task<ExchangeRate?> GetExchangeRateAsync();
}