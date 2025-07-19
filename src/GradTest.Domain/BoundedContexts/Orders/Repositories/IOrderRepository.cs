using GradTest.Domain.BoundedContexts.Orders.Entities;
using GradTest.Domain.Common.Contracts;

namespace GradTest.Domain.BoundedContexts.Orders.Repositories;

public interface IOrderRepository : IRepository
{
    public Task<Order?> TryGetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    public Task<List<OrderItem>?> TryGetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
}