namespace GradTest.Contracts.Products.Requests;

public class UpdateProductRequest
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public decimal? Price { get; init; }
    public int? Quantity { get; init; }
    public string? CategoryName { get; init; }
}