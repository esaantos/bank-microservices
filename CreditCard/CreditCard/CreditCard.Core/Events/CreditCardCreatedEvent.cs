namespace CreditCards.Core.Events;

public record CreditCardCreatedEvent
{
    public List<int> CreditCardId { get;  }
    public int CustomerId { get; }

    public CreditCardCreatedEvent(List<int> creditCardId, int customerId)
    {
        CreditCardId = creditCardId;
        CustomerId = customerId;
    }
}
