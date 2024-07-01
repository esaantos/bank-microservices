using MediatR;

namespace Customers.Core.Events;

public record CreditProposalCreatedEvent : INotification
{
    public CreditProposalCreatedEvent(int customerId, decimal creditValue)
    {
        CustomerId = customerId;
        CreditValue = creditValue;
    }

    public int CustomerId { get; }
    public decimal CreditValue { get; }
}
