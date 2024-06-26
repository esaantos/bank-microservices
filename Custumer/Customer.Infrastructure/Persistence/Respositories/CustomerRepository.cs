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
        return await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);
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

    public async Task<bool> AddCustomerCredit(Customer customer)
    {
        return true;
    }

}
