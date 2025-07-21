using FluentValidation;
using GradTest.Application.BoundedContexts.Orders.Mapping;
using GradTest.Contracts.Orders.Responses;
using GradTest.Domain.BoundedContexts.Orders.Entities;
using GradTest.Domain.BoundedContexts.Orders.Repositories;
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
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        
        public async Task<Result<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var createOrderResult = Order.Create(request.UserId, request.Items);

            _orderRepository.Add(createOrderResult);
            var saveResult = await _orderRepository.SaveChangesAsync(cancellationToken);
            
            if (saveResult.IsError)
            {
                return Result.Error(saveResult.ErrorValue);
            }
            
            var response = createOrderResult.ToResponse();
        
            return Result<OrderResponse>.Success(response);
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