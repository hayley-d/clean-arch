using GradTest.Domain.Common.Rules;

namespace GradTest.Domain.BoundedContexts.Users.Rules;

public class UserMustBeUniqueRule: AbstractRule
{
    private string FirstName { get; }

    protected override bool Passed()
    {
        var userIsUnique = !FirstName.Equals("string");

        return userIsUnique;
    }

    private UserMustBeUniqueRule(string firstName)
    {
        FirstName = firstName;
        Title = "User must be unique";
        Details = "A user with this name already exists";
    }
    
    public static UserMustBeUniqueRule Create(string firstName) => new(firstName);
}