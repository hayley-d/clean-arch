using GradTest.Application.BoundedContexts.Orders.Mapping;
using GradTest.Application.Common.Contracts;
using GradTest.Contracts.Orders.Responses;
using GradTest.Domain.BoundedContexts.OrderItems.Repositories;
using GradTest.Domain.BoundedContexts.Orders.Repositories;
using GradTest.Shared.Errors;
using GradTest.Shared.Monads;
using MediatR;

namespace GradTest.Application.BoundedContexts.Orders.Queries;

public class GetOrderByIdQuery : IQuery<Result<OrderResponse>>
{
    
    public Guid OrderId { get; }

    public GetOrderByIdQuery(Guid orderId)
    {
        OrderId = orderId;
    }
    
    internal sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        
        public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }
        
        public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.TryGetByIdAsync(request.OrderId, cancellationToken);
            
            if (order is null)
            {
                return Result.Error(GenericError.Create("Order not found", "Order not found in the database."));
            }

            var items = await _orderItemRepository.GetOrderItemsAsync(order.Id, cancellationToken);
            
            var responseItems = items.Select(orderItem => OrderExtensions.ToOrderItemResponse(orderItem)).ToList();

            var orderResponse = OrderExtensions.ToResponse(order);

            return Result<OrderResponse>.Success(orderResponse);
        }
    }
}