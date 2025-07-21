using GradTest.Application.BoundedContexts.Products.Mapping;
using GradTest.Application.Common.Contracts;
using GradTest.Contracts.Products.Responses;
using GradTest.Domain.BoundedContexts.Products.Repositories;
using GradTest.Shared.Monads;
using MediatR;

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
            var items = await _productRepository.ListAsync(cancellationToken);
            var response  = items.Select(item => item.ToResponse()).ToList();

            return Result<List<ProductResponse>>.Success(response);
        }
    }
}