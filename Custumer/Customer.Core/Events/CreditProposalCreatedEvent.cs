using MediatR;

namespace Customers.Core.Events;

public record CreditProposalCreatedEvent : INotification
{
    public CreditProposalCreatedEvent(int customerId, int proposalId, bool success)
    {
        CustomerId = customerId;
        ProposalId = proposalId;
        Success = success;
    }

    public int CustomerId { get; }
    public int ProposalId { get; }
    public bool Success { get; set; }
}
