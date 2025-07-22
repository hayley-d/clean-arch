using GradTest.Application.BoundedContexts.Orders.Mapping;
using GradTest.Application.Common.Contracts;
using GradTest.Contracts.Orders.Responses;
using GradTest.Domain.BoundedContexts.ExchangeRates.Repositories;
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
        private readonly IExchangeRateRepository _exchangeRateRepository;
        
        public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IExchangeRateRepository exchangeRateRepository)
        {
            _orderRepository = orderRepository;
            _exchangeRateRepository = exchangeRateRepository;
        }
        
        public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            
            var order = await _orderRepository.TryGetByIdAsync(request.OrderId, cancellationToken);
            
            if (order is null)
            {
                return Result.Error(GenericError.Create("Order not found", "Order not found in the database."));
            }

            var rate = await _exchangeRateRepository.GetLatestExchangeRate(cancellationToken);

            if (rate is null or 0)
            {
                return Result.Error(GenericError.Create("Unable to find exchange rate", "Failed to find exchange rate"));
            }
            
            var orderResponse = order.ToResponse(rate.Value);

            return Result<OrderResponse>.Success(orderResponse);
        }
    }
}