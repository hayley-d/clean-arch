namespace GradTest.Contracts.Users.Requests;

public class CreateUserRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}