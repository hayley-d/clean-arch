using FluentValidation;
using GradTest.Application.BoundedContexts.Orders.Mapping;
using GradTest.Application.Common.Contracts;
using GradTest.Application.Common.Services;
using GradTest.Contracts.Orders.Requests;
using GradTest.Contracts.Orders.Responses;
using GradTest.Domain.BoundedContexts.ExchangeRates.Repositories;
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
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IExchangeRateService _exchangeRateService;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository, IExchangeRateRepository exchangeRateRepository, IExchangeRateService exchangeRateService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _exchangeRateService = exchangeRateService;
        }
        
        public async Task<Result<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var rate = await _exchangeRateRepository.GetLatestExchangeRate(cancellationToken);

            if (rate is null or 0)
            {
                var liveRate = await _exchangeRateService.GetExchangeRateAsync();
            
                if (liveRate is null)
                    throw new NullReferenceException("Exchange rate not available.");        
            
                _exchangeRateRepository.Add(liveRate);
            
                await _exchangeRateRepository.SaveChangesAsync(cancellationToken);
            
                rate = liveRate.ZAR;
            }
            
            var itemsSet = new Dictionary<Guid, int>();

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                {
                    return Result.Error(GenericError.Create("Invalid product quantity", "Quantity value must be greater than zero"));
                }
                
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
            
            var response = createOrderResult.ToResponse(rate.Value);
        
            return Result<OrderResponse>.Success(response);
        }
    }
}

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();
        
        RuleFor(command => command.Items)
            .NotEmpty();

        RuleForEach(command => command.Items).ChildRules(item =>
        {
            item.RuleFor(orderItem => orderItem.Quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(10000);
        });
    }
}