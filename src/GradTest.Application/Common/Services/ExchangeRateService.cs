using System.Net.Http.Json;
using GradTest.Domain.Common.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GradTest.Application.Common.Services;

public class ExchangeRateService: IExchangeRateService
{
   private class OpenExchangeResponse
   {
      public string Base { get; set; } = "";
      public Dictionary<string, decimal> Result { get; set; } = new();
      public string Updated { get; set; } = "";
      public decimal Rate { get; set; }
   }
   
   private readonly HttpClient _client;
   private readonly IConfiguration? _config;
   private readonly ILogger<ExchangeRateService> _logger;

   public ExchangeRateService(HttpClient client, ILogger<ExchangeRateService> logger, IConfiguration? config = null)
   {
      _client = client;
      _config = config;
      _logger = logger;
   }

   public async Task<ExchangeRate?> GetExchangeRateAsync()
   {
      try
      {
         var apiKey = _config?["ExchangeApiKey"];

         if (string.IsNullOrWhiteSpace(apiKey))
         {
            _logger.LogError("API key for Open Exchange Rates not found.");
            
            return null;
         }

         var url = $"https://api.fastforex.io/fetch-one?from=USD&to=ZAR&api_key={apiKey}";

         var httpResponse = await _client.GetAsync(url);

         if (!httpResponse.IsSuccessStatusCode)
         {
            _logger.LogError($"Failed to fetch exchange rates: {httpResponse.StatusCode}");
            
            return null;
         }

         var rawJson = await httpResponse.Content.ReadAsStringAsync();
         
         _logger.LogInformation("Raw exchange rate JSON: " + rawJson);
         
         var response = await httpResponse.Content.ReadFromJsonAsync<OpenExchangeResponse>();

         if (response is null || !response.Result.TryGetValue("ZAR", out var zar))
         {
            _logger.LogError("Failed to find ZAR in exchange rate response.");
            
            return null;
         }

         return new ExchangeRate(zar);
      }
      catch (HttpRequestException ex)
      {
         _logger.LogError($"Network error while fetching exchange rates: {ex.Message}");
         
         return null;
      }
      catch (Exception ex)
      {
         _logger.LogError($"Unexpected error while fetching exchange rates: {ex.Message}");
         
         return null;
      }
   }
}