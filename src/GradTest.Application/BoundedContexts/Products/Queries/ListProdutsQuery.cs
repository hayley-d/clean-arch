using GradTest.Contracts.Products.Responses;
using GradTest.Domain.BoundedContexts.Products.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http.Metadata;

namespace GradTest.Application.BoundedContexts.Products.Queries;

public class ListProdutsQuery : IQuery<Result<List<ProductResponse>>>
{
    internal sealed class ListProductQueryHandler : IRequestHandler<ListProdutsQuery, Result<List<ProductResponse>>>
    {
        private readonly IProductRepository _productRepository;

        public ListProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        public async Task<Result<List<ProductResponse>>> Handle(ListProdutsQuery request, CancellationToken cancellationToken)
        {
            var items = await _productRepository.ListAsync();
            var response  = new List<ProductResponse>();
            
            foreach (var item in items)
            {
                response.Add(new ProductResponse
                {
                    ProductId = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Category = item.Category.Name
                });
            }
            return Result<List<ProductResponse>>.Success(response);
        }
    }
}