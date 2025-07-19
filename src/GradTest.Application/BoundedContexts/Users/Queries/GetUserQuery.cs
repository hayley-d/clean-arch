using FluentValidation;
using GradTest.Application.Common.Contracts;
using MediatR;
using GradTest.Contracts.Users.Responses;
using GradTest.Domain.BoundedContexts.Users.Entities;
using GradTest.Domain.BoundedContexts.Users.Repositories;
using GradTest.Domain.BoundedContexts.Users.ValueObjects;

namespace GradTest.Application.BoundedContexts.Users.Queries;

public class GetUserQuery: IQuery<Result<UserResponse>>
{
    public UserId UserId { get; }

    public GetUserQuery(UserId userId)
    {
        UserId = userId;
    }
}

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.UserId.Value).NotEmpty();
    }
}

internal sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        await _userRepository.TryGetByIdAsync(request.UserId, cancellationToken);

        var rng = new Random((int)DateTime.Now.Ticks);
        var x = rng.Next(0, 10);
        var even = x % 2 == 0;
        
        var dto = new UserResponse
        {
            Id = Guid.NewGuid(),
            FirstName = "some name",
            LastName = "Some lastname"
        };

        return even switch
        {
            true => Result<UserResponse>.Success(dto),
            false => Result<UserResponse>.Error(NotFoundError.Create(nameof(User), request.UserId.ToString()))
        };
    }
}