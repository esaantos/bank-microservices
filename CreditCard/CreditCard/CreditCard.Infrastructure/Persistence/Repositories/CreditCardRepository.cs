using CreditCards.Core.Entities;
using CreditCards.Core.Repositories;

namespace CreditCards.Infrastructure.Persistence.Repositories;

public class CreditCardRepository : ICreditCardRepository
{
    private readonly CreditCardContext _context;

    public CreditCardRepository(CreditCardContext context)
    {
        _context = context;
    }

    public async Task<CreditCard> GetByIdAsync(int id)
    {
        return await _context.CreditCards.FindAsync(id);
    }
    public async Task AddAsync(List<CreditCard> creditCards)
    {
        _context.CreditCards.AddRangeAsync(creditCards);
        await _context.SaveChangesAsync();
    }
}
