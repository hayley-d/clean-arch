using GradTest.Domain.BoundedContexts.Users.Rules;
using GradTest.Domain.BoundedContexts.Users.ValueObjects;
using GradTest.Domain.Common.Entities;
using GradTest.Domain.Common.Rules;

namespace GradTest.Domain.BoundedContexts.Users.Entities;

public class User: EntityBase
{
    public UserId Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    

    private User(string firstName, string lastName)
    {
        Id = UserId.New();
        FirstName = firstName;
        LastName = lastName;
    }

    private User() { }

    public static Result<User> Create(string firstName, string lastName)
    {
        var ruleResult = RuleValidator.ResultFrom(UserMustBeUniqueRule.Create(firstName));

        if (ruleResult.IsError)
        {
            return ruleResult;
        }

        var user = new User(firstName, lastName); 
        
        return Result<User>.Success(user);
    }
}