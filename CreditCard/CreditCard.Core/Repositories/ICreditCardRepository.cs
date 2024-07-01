using CreditCards.Core.Entities;

namespace CreditCards.Core.Repositories;
public interface ICreditCardRepository
{
    Task AddAsync(CreditCard creditCards);
    Task<CreditCard> GetByIdAsync(int id);
    Task<IEnumerable<CreditCard>> GetCreditCardsByCustomerIdAsync(int id);
}
