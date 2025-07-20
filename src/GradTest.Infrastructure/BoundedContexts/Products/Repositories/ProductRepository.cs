using System.ComponentModel;
using GradTest.Domain.BoundedContexts.Products.Entities;
using GradTest.Infrastructure.Common.Repository;
using GradTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GradTest.Infrastructure.BoundedContexts.Products.Repositories;

public class ProductRepository : BaseRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context) { }
    
    public Task<Product?> TryGetByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return Context.Products.FirstOrDefaultAsync(product => product.Id == productId, cancellationToken);
    }
    
    public Task<Product?> TryGetByNameAsync(Guid productName, CancellationToken cancellationToken = default)
    {
        return Context.Products.FirstOrDefaultAsync(product => product.Name.Equals(productName), cancellationToken);
    }
}