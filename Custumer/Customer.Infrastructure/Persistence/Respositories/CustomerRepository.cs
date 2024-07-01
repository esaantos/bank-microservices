using Customers.Core.Entities;
using Customers.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Customers.Infrastructure.Persistence.Respositories;

public class CustomersRepository : ICustomersRepository
{
    private readonly CustomerContext _context;

    public CustomersRepository(CustomerContext context)
    {
        _context = context;
    }

    public async Task<Customer> GetByIdAsync(int id)
    {
        return await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Customer customers)
    {
        await _context.Customers.AddAsync(customers);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCreditCardNumbersAsync(Customer customer)
    {
        try
        {
            _context.Entry(customer).Property(c => c.CreditCardNumbers).IsModified = true;
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            // Detach a entidade atual para evitar conflitos de rastreamento
            _context.Entry(customer).State = EntityState.Detached;
            throw;
        }  

    }

    public async Task UpdateCreditProposalsAsync(Customer customer)
    {
        try
        {
            _context.Entry(customer).Property(c => c.CreditProposalValue).IsModified = true;
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            // Detach a entidade atual para evitar conflitos de rastreamento
            _context.Entry(customer).State = EntityState.Detached;
            throw;
        }
        
    }
}
