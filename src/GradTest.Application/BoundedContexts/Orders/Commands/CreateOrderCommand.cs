using FluentValidation;
using GradTest.Application.BoundedContexts.Orders.Mapping;
using GradTest.Contracts.Orders.Responses;
using GradTest.Domain.BoundedContexts.Orders.Entities;
using MediatR;

namespace GradTest.Application.BoundedContexts.Orders.Commands;

public class CreateOrderCommand : ICommand<Result<OrderResponse>>
{
    public Guid UserId { get; init; }
    public List<OrderItem> Items { get; init; }

    public CreateOrderCommand(Guid userId, List<OrderItem> items)
    {
        UserId = userId;
        Items = items;
    }

    internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderResponse>>
    {
        public Task<Result<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var createOrderResult = Order.Create(request.UserId, request.Items);

            var response = createOrderResult.ToResponse();
        
            return Task.FromResult<Result<OrderResponse>>(response);
        }
    }
}

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Items)
            .NotNull()
            .NotEmpty();
    }
}