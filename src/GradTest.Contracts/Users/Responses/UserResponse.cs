namespace GradTest.Contracts.Users.Responses;

public class UserResponse
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}