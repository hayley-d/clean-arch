using Ardalis.SmartEnum;

namespace GradTest.Domain.BoundedContexts.Users.Roles;

public abstract class Role: SmartEnum<Role>
{
    public static readonly Role Admin = new AdminRole();
    public static readonly Role Unknown = new UnknownRole();
    
    private Role(string name, int value) : base(name, value) { }

    private sealed class AdminRole() : Role("Admin", 0);

    private sealed class UnknownRole() : Role("Unknown", 999);
}