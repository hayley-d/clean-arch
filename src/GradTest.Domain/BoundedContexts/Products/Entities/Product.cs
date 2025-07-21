using GradTest.Domain.BoundedContexts.Products.Enums;
using GradTest.Shared.Monads;

namespace GradTest.Domain.BoundedContexts.Products.Entities;

public class Product
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Category Category { get; init; }
    
    private Product() { }

    private Product(string name, string description, decimal price, int quantity, Category category)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        Quantity = quantity;
        Category = category;
    }

    public static Result<Product> Create(string name, string description, decimal price, int quantity, Category cateogry)
    {
        var product = new Product(name, description, price, quantity, cateogry); 
        
        return Result<Product>.Success(product);
    }
}