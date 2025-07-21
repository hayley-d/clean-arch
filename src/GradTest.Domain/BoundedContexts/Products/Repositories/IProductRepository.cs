using GradTest.Domain.BoundedContexts.Products.Entities;
using GradTest.Domain.Common.Contracts;

namespace GradTest.Domain.BoundedContexts.Products.Repositories;

public interface IProductRepository : IRepository
{
    public Task<Product?> TryGetByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    public Task<List<Product>> ListAsync(CancellationToken cancellationToken = default); 
}