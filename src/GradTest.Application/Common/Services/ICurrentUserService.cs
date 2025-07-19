using GradTest.Domain.BoundedContexts.Users.Roles;
using GradTest.Domain.BoundedContexts.Users.ValueObjects;

namespace GradTest.Application.Common.Services;

public interface ICurrentUserService
{
    HashSet<Role> GetRolesFromToken();
    UserId GetUserIdFromToken();
}