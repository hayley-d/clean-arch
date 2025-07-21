namespace GradTest.Domain.Common.Entities;

public class ExchangeRate
{
    public Guid Id { get; init; }
    public decimal USD { get; init; }
    public decimal ZAR { get; init; }
    public DateTime Date { get; init; }
    
    public ExchangeRate(){}

    public ExchangeRate(decimal zar)
    {
        Id = Guid.NewGuid();
        USD = 1;
        ZAR = zar;
        Date = DateTime.UtcNow;
    }
}