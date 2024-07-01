using CreditCards.Core.Entities;
using CreditsProposal.Core.Events;

namespace CreditsProposal.Core.Entities;

public class CreditProposal : AggregateRoot
{
    public CreditProposal(int customerId, decimal creditValue)
    {
        CustomerId = customerId;
        CreditValue = creditValue;

        AddEvent(new CreditProposalCreatedEvent(customerId, creditValue));
    }

    public int CustomerId { get; set; }
    public decimal CreditValue { get; private set; }

    public static decimal GenerateCredit(decimal income)
    {
        return income * 3;
    }
}


