using GradTest.Contracts.Products.Responses;
using MediatR;

namespace GradTest.Application.BoundedContexts.Products.Queries;

public class GetProductByIdQuery(Guid ProductId) : IQuery<Result<ProductResponse>>
{
    internal sealed class GetProductByQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
    {
        public Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<ProductResponse>>(new ProductResponse());
        }
    }
}

public class GetMyUserQuery: IQuery<Result<Guid>>;

internal sealed class GetMyUserQueryHandler : IRequestHandler<GetMyUserQuery, Result<Guid>>
{
    public Task<Result<Guid>> Handle(GetMyUserQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<Guid>>(Guid.NewGuid());
    }
}