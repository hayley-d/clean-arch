using GradTest.Contracts.Products.Responses;
using MediatR;

namespace GradTest.Application.BoundedContexts.Products.Queries;

public class ListProdutsQuery : IQuery<Result<List<ProductResponse>>>
{
    internal sealed class ListProductQueryHandler() : IRequestHandler<ListProdutsQuery, Result<List<ProductResponse>>>
    {
        public Task<Result<List<ProductResponse>>> Handle(ListProdutsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<ProductResponse>>>(new List<ProductResponse>());
        }
    }
}