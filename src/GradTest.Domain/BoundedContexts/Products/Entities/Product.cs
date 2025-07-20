using GradTest.Domain.BoundedContexts.Products.Enums;
using GradTest.Domain.BoundedContexts.Products.Rules;
using GradTest.Domain.Common.Rules;
using GradTest.Shared.Monads;

namespace GradTest.Domain.BoundedContexts.Products.Entities;

public class Product
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; init; }
    public Category Cateogry { get; init; }
    
    private Product() { }

    private Product(string name, string description, decimal price, int quantity, Category cateogry)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        Quantity = quantity;
        Cateogry = cateogry;
    }

    public static Result<Product> Create(string name, string description, decimal price, int quantity, Category cateogry)
    {
        
        var ruleResult = RuleValidator.ResultFrom(ProductMustBeUniqueRule.Create(name));

        if (ruleResult.IsError)
        {
            return ruleResult;
        }

        var product = new Product(name, description, price, quantity, cateogry); 
        
        return Result<Product>.Success(product);
    }
}