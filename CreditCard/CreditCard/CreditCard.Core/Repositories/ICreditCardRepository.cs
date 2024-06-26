using CreditCards.Core.Entities;

namespace CreditCards.Core.Repositories;
public interface ICreditCardRepository
{
    Task AddAsync(List<CreditCard> creditCards);
    Task<CreditCard> GetByIdAsync(int id);
}
