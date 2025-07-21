using GradTest.Domain.BoundedContexts.OrderItems.Repositories;
using GradTest.Domain.BoundedContexts.Orders.Entities;
using GradTest.Infrastructure.Common.Repository;
using GradTest.Infrastructure.Persistence;

namespace GradTest.Infrastructure.BoundedContexts.OrderItems.Repositories;

public class OrderItemRepository : BaseRepository, IOrderItemRepository 
{
    private readonly ApplicationDbContext _context;
    
    public OrderItemRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<List<OrderItem>> GetOrderItemsAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var items = _context.OrderItems.Where(x => x.OrderId == orderId).ToList();
        return Task.FromResult(items);
    }
}