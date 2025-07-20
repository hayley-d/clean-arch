using GradTest.Contracts.Orders.Responses;
using GradTest.Domain.BoundedContexts.Orders.Entities;
using MediatR;

namespace GradTest.Application.BoundedContexts.Orders.Queries;

public class GetOrderByIdQuery(Guid orderId) : IQuery<Result<OrderResponse>>
{
    internal sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
    {
        public Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<OrderResponse>>(new OrderResponse());
        }
    }
}