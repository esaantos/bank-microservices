namespace CreditsProposal.Core.Events;

public record CreditProposalCreatedEvent
{
    public CreditProposalCreatedEvent(int customerId, int? proposalId)
    {
        CustomerId = customerId;
        ProposalId = proposalId;
    }

    public int CustomerId { get; set; }
    public int? ProposalId { get; set; }
}
