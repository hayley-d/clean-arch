using GradTest.Domain.BoundedContexts.Orders.Entities;
using GradTest.Domain.Common.Contracts;

namespace GradTest.Domain.BoundedContexts.OrderItems.Repositories;

public interface IOrderItemRepository : IRepository
{
    public Task<List<OrderItem>> GetOrderItemsAsync(Guid orderId, CancellationToken cancellationToken = default);
}