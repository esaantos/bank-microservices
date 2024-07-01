using CreditsProposal.Core.Entities;
using CreditsProposal.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CreditsProposal.Infrastructure.Persistence.Repositories;

public class CreditProposalRepository : ICreditProposalRepository
{
    private readonly CreditProposalContext _context;

    public CreditProposalRepository(CreditProposalContext context)
    {
        _context = context;
    }

    public async Task<CreditProposal> GetByIdAsync(int id)
    {
        return await _context.CreditProposals.FindAsync(id);
    }
    public async Task AddAsync(CreditProposal creditsProposal)
    {
        await _context.CreditProposals.AddAsync(creditsProposal);
        await _context.SaveChangesAsync();
    }

    public async Task<CreditProposal> GetCreditCardsByCustomerIdAsync(int customerId)
    {
        return await _context.CreditProposals.FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }
}
