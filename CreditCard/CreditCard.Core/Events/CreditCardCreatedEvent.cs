namespace CreditCards.Core.Events;

public class CreditCardCreatedEvent : IDomainEvent
{
    public int CustomerId { get; }
    public string CardHolderName { get; }
    public string CardNumber { get; }
    public DateTime ExpirationDate { get; }
    public string CVV { get; }

    public CreditCardCreatedEvent(int customerId, string cardHolderName, string cardNumber, DateTime expirationDate, string cvv)
    {
        CustomerId = customerId;
        CardHolderName = cardHolderName;
        CardNumber = cardNumber;
        ExpirationDate = expirationDate;
        CVV = cvv;
    }
}
