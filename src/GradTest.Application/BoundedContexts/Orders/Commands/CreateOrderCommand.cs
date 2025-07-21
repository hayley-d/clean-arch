using FluentValidation;
using GradTest.Application.BoundedContexts.Orders.Mapping;
using GradTest.Application.Common.Contracts;
using GradTest.Contracts.Orders.Requests;
using GradTest.Contracts.Orders.Responses;
using GradTest.Domain.BoundedContexts.Orders.Entities;
using GradTest.Domain.BoundedContexts.Orders.Repositories;
using GradTest.Domain.BoundedContexts.Products.Repositories;
using GradTest.Shared.Errors;
using GradTest.Shared.Monads;
using MediatR;

namespace GradTest.Application.BoundedContexts.Orders.Commands;

public class CreateOrderCommand : ICommand<Result<OrderResponse>>
{
    public Guid UserId { get; init; }
    public List<OrderItemRequest> Items { get; init; }

    public CreateOrderCommand(Guid userId, List<OrderItemRequest> items)
    {
        UserId = userId;
        Items = items;
    }

    internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }
        
        public async Task<Result<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {

            var itemsSet = new Dictionary<Guid, int>();

            foreach (var item in request.Items)
            {
                itemsSet.Add(item.ProductId, item.Quantity);
                var product = await _productRepository.TryGetByIdAsync(item.ProductId, cancellationToken);
                
                if (product is null)
                {
                    return Result.Error(GenericError.Create("Product not found", "Product not found"));
                }

                if (product.Quantity < item.Quantity)
                {
                    return Result.Error(GenericError.Create("Insufficient product quantity", "Insufficient quantity"));                   
                }
                
                product.Quantity -= item.Quantity;
                _productRepository.Update(product);
                await _productRepository.SaveChangesAsync(cancellationToken);
            }
            
            var createOrderResult = Order.Create(request.UserId, itemsSet);

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