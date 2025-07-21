using GradTest.Domain.BoundedContexts.Orders.Entities;
using GradTest.Domain.BoundedContexts.Orders.Repositories;
using GradTest.Infrastructure.Common.Repository;
using GradTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GradTest.Infrastructure.BoundedContexts.Ordrs.Repositories;

public class OrderRepository: BaseRepository, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context) { }
    
    public Task<Order?> TryGetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return Context.Orders.FirstOrDefaultAsync(order => order.Id == orderId, cancellationToken);
    }

    public Task<List<OrderItem>?> TryGetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return Context.OrderItems.Where(orderItem => orderItem.OrderId == orderId).ToListAsync();
    }
}