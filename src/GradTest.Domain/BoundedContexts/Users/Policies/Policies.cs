using GradTest.Domain.BoundedContexts.Users.Roles;

namespace GradTest.Domain.BoundedContexts.Users.Policies;

public static class Policies
{
    public const string Admin = nameof(Role.Admin);
    public const string AnyRole = "Any";
}