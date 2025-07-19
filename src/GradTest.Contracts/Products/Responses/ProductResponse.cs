namespace GradTest.Contracts.Products.Responses;

public class ProductResponse
{
    public Guid ProductId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; init; }
    public string Category { get; init; }
}