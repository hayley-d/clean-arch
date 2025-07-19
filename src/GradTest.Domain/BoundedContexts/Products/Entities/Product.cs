using GradTest.Domain.BoundedContexts.Products.Enums;

namespace GradTest.Domain.BoundedContexts.Products.Entities;

public class Product
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; init; }
    public Category Cateogry { get; init; }
}