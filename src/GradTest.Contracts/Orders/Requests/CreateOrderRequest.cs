namespace GradTest.Contracts.Orders.Requests;

public class CreateOrderRequest
{
    public Dictionary<Guid, int> Items { get; init; }
}