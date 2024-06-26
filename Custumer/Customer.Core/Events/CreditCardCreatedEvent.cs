using MediatR;

namespace Customers.Core.Events;

public record CreditCardCreatedEvent : INotification
{
    public CreditCardCreatedEvent(List<int> creditCardId, int customerId, bool success)
    {
        CreditCardId = creditCardId;
        CustomerId = customerId;
        Success = success;
    }

    public List<int> CreditCardId { get; }
    public int CustomerId { get; }
    public bool Success { get; }
}
