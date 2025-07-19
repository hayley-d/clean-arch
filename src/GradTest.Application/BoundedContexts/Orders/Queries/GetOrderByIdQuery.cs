using GradTest.Contracts.Orders.Responses;
using MediatR;

namespace GradTest.Application.BoundedContexts.Orders.Queries;

public class GetOrderByIdQuery
{
    
}

public class GetOrderByIdQueryValidator : IQuery<Result<OrderResponse>>
{
    
}

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
{
    
}