using GradTest.Contracts.Products.Responses;
using GradTest.Domain.BoundedContexts.Products.Repositories;
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

            var response = new ProductResponse
            {
                ProductId = product.Id,
                Name = product?.Name,
                Description = product?.Description,
                Price = (decimal)product?.Price,
                Quantity = (int)product?.Quantity,
                Category = product?.Category.Name,

            };
            
            return Result<ProductResponse>.Success(response);
        }
    }
}