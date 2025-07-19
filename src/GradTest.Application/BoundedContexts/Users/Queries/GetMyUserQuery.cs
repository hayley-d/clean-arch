using GradTest.Application.Common.Contracts;
using MediatR;

namespace GradTest.Application.BoundedContexts.Users.Queries;

public class GetMyUserQuery: IQuery<Result<Guid>>;

internal sealed class GetMyUserQueryHandler : IRequestHandler<GetMyUserQuery, Result<Guid>>
{
    public Task<Result<Guid>> Handle(GetMyUserQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<Guid>>(Guid.NewGuid());
    }
}