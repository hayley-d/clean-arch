using GradTest.Application.BoundedContexts.Products.Mapping;
using GradTest.Contracts.Products.Responses;
using GradTest.Domain.BoundedContexts.Products.Entities;
using GradTest.Domain.BoundedContexts.Products.Enums;
using GradTest.Domain.BoundedContexts.Products.Repositories;
using MediatR;

namespace GradTest.Application.BoundedContexts.Products.Commands;

public class CreateProductCommand : ICommand<Result<ProductResponse>>
{
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; init; }
    public Category Category { get; init; }

    public CreateProductCommand(string name, string description, decimal price, int quantity, Category category)
    {
        Name = name;
        Description = description;
        Price = price;
        Quantity = quantity;
        Category = category;
    }

    internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository  productRepository)
        {
            _productRepository = productRepository;
        }
        
        public Task<Result<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var createProductrResult = Product.Create(request.Name, request.Description, request.Price, request.Quantity, request.Category);
            
            if (createProductrResult.IsError)
            {
                var errorResult = Result.Error(createProductrResult.ErrorValue);
                return Task.FromResult<Result<ProductResponse>>(errorResult);
            }
            
            _productRepository.Add(createProductrResult.Value);
            
            var response = createProductrResult.Value.ToResponse();
        
            return Task.FromResult<Result<ProductResponse>>(response);
        }
    }
}