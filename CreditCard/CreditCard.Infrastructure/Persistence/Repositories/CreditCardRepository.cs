using CreditCards.Core.Entities;
using CreditCards.Core.Repositories;
using Microsoft.EntityFrameworkCore;

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
    public async Task AddAsync(CreditCard creditCard)
    {
        await _context.CreditCards.AddAsync(creditCard);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CreditCard>> GetCreditCardsByCustomerIdAsync(int id)
    {
        return await _context.CreditCards.Where(c => c.CustomerId == id).ToListAsync();
    }
}
