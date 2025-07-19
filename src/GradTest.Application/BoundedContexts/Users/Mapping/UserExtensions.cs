using GradTest.Contracts.Users.Responses;
using GradTest.Domain.BoundedContexts.Users.Entities;

namespace GradTest.Application.BoundedContexts.Users.Mapping;

public static class UserExtensions
{
    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id.Value,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}