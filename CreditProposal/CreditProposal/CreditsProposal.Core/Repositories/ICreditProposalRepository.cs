using CreditsProposal.Core.Entities;

namespace CreditsProposal.Core.Repositories;

public interface ICreditProposalRepository
{
    Task<CreditProposal> GetByIdAsync(int id);
    Task AddAsync(CreditProposal creditsProposal);
}
