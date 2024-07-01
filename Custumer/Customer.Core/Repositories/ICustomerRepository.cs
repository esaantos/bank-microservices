using Customers.Core.Entities;

namespace Customers.Core.Repositories;
public interface ICustomersRepository
{
    Task<Customer> GetByIdAsync(int id);
    Task AddAsync(Customer Customers);
    Task UpdateAsync(Customer customer);
    Task UpdateCreditCardNumbersAsync(Customer customer);
    Task UpdateCreditProposalsAsync(Customer customer);
}
