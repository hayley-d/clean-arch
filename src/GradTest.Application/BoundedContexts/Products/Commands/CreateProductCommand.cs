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
        
        public async Task<Result<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var createProductrResult = Product.Create(
                request.Name, 
                request.Description, 
                request.Price, 
                request.Quantity, 
                request.Category
            );
            
            if (createProductrResult.IsError)
            {
                return Result.Error(GenericError.Create("Product creation error", "Product could not be created."));
            }

            var product = createProductrResult.Value;
            
            _productRepository.Add(product);
            var saveResult = await _productRepository.SaveChangesAsync(cancellationToken);
            
            if (saveResult.IsError)
            {
                return Result.Error(saveResult.ErrorValue);
            }

            var response = new ProductResponse
            {
                ProductId = createProductrResult.Value.Id,
                Name = createProductrResult.Value.Name,
                Description = createProductrResult.Value.Description,
                Price = createProductrResult.Value.Price,
                Quantity = createProductrResult.Value.Quantity,
                Category = createProductrResult.Value.Category.Name
            };
        
            return Result<ProductResponse>.Success(response);
        }
    }
}