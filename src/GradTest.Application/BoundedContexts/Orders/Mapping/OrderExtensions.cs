using GradTest.Contracts.Orders.Responses;
using GradTest.Domain.BoundedContexts.Orders.Entities;

namespace GradTest.Application.BoundedContexts.Orders.Mapping;

public static class OrderExtensions
{
    public static OrderResponse ToResponse(this Order order)
    {
        return new OrderResponse
        {
            OrderId = order.Id,
            CustomerId = order.UserId,
            Items = order.Items,
        };
    }
}
