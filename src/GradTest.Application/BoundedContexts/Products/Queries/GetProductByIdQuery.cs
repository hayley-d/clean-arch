using GradTest.Application.BoundedContexts.Products.Mapping;
using GradTest.Application.Common.Contracts;
using GradTest.Contracts.Products.Responses;
using GradTest.Domain.BoundedContexts.Products.Repositories;
using GradTest.Shared.Errors;
using GradTest.Shared.Monads;
using MediatR;

namespace GradTest.Application.BoundedContexts.Products.Queries;

public class GetProductByIdQuery : IQuery<Result<ProductResponse>>
{
    
    public Guid ProductId { get; set; }

    public GetProductByIdQuery(Guid productId)
    {
        ProductId = productId;
    }
    
    internal sealed class GetProductByQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.TryGetByIdAsync(request.ProductId, cancellationToken);
            
            if (product is null)
            {
                return Result.Error(GenericError.Create("Product not found", "Product not found in the database."));
            }

            var response = product.ToResponse();
            
            return Result<ProductResponse>.Success(response);
        }
    }
}