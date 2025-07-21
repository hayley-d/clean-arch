using FluentValidation;
using GradTest.Application.BoundedContexts.Products.Mapping;
using GradTest.Application.Common.Contracts;
using GradTest.Contracts.Products.Responses;
using GradTest.Domain.BoundedContexts.Products.Entities;
using GradTest.Domain.BoundedContexts.Products.Enums;
using GradTest.Domain.BoundedContexts.Products.Repositories;
using GradTest.Shared.Errors;
using GradTest.Shared.Monads;
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

            var response = product.ToResponse();
        
            return Result<ProductResponse>.Success(response);
        }
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .MaximumLength(50).WithMessage("Name is too long")
            .MinimumLength(3).WithMessage("Name is too short");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty")
            .MaximumLength(500).WithMessage("Description is too long")
            .MinimumLength(3).WithMessage("Description is too short");
        
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price cannot be negative")
            .LessThan(10000).WithMessage("Price value is too large.")
            .NotEmpty().WithMessage("Price cannot be empty");
        
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity cannot be negative")
            .LessThan(10000).WithMessage("Quantity value is too large.")
            .NotEmpty().WithMessage("Quantity cannot be empty");
        
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category cannot be empty");
    }
}
