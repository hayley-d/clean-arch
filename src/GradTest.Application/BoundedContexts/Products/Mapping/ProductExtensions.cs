using GradTest.Contracts.Products.Responses;
using GradTest.Domain.BoundedContexts.Products.Entities;

namespace GradTest.Application.BoundedContexts.Products.Mapping;

public static class ProductExtensions
{
    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse
        {
            ProductId = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity,
            Category = product.Category.Name,
        };
    }
}