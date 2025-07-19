using FluentValidation;
using GradTest.Application.Common.Contracts;
using MediatR;
using GradTest.Application.BoundedContexts.Users.Mapping;
using GradTest.Contracts.Users.Responses;
using GradTest.Domain.BoundedContexts.Users.Entities;

namespace GradTest.Application.BoundedContexts.Users.Commands;

public class CreateUserCommand: ICommand<Result<UserResponse>>
{
    public string FirstName { get; }
    public string LastName { get; }

    public CreateUserCommand(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
    }
}

internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserResponse>>
{
    public Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var createUserResult = User.Create(request.FirstName, request.LastName);

        if (createUserResult.IsError)
        {
            var errorResult = Result.Error(createUserResult.ErrorValue);
            return Task.FromResult<Result<UserResponse>>(errorResult);
        }

        var response = createUserResult.Value.ToResponse();
        
        return Task.FromResult<Result<UserResponse>>(response);
    }
}