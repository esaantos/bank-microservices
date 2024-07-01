namespace CreditsProposal.Core.Events;

public class CreditProposalCreatedEvent : IDomainEvent
{
    public CreditProposalCreatedEvent(int customerId, decimal creditValue)
    {
        CustomerId = customerId;
        CreditValue = creditValue;
    }

    public int CustomerId { get; }
    public decimal CreditValue { get; }
}
