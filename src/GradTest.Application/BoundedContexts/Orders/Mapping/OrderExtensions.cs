using GradTest.Contracts.Orders.Responses;
using GradTest.Domain.BoundedContexts.Orders.Entities;

namespace GradTest.Application.BoundedContexts.Orders.Mapping;

public static class OrderExtensions
{
    public static OrderResponse ToResponse(this Order order)
    {
        var responseItems = order.Items.Select(item => ToOrderItemResponse(item)).ToList();

        return new OrderResponse
        {
            OrderId = order.Id,
            CustomerId = order.UserId,
            Items = responseItems,
        };
    }

    public static OrderItemResponse ToOrderItemResponse(this OrderItem orderItem)
    {
        return new OrderItemResponse
        {
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
        };
    }
}
