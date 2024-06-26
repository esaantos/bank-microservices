using Customers.Core.Entities;

namespace Customers.Core.Repositories;
public interface ICustomersRepository
{
    Task<Customer> GetByIdAsync(int id);
    Task AddAsync(Customer Customers);
    Task<bool> AddCustomerCredit(Customer customer);
    Task UpdateAsync(Customer customer);
}
