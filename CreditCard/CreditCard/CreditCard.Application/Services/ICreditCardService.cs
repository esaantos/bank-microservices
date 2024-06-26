using CreditCards.Core.Entities;
using CreditCards.Core.Events;

namespace CreditCards.Application.Services;

public interface ICreditCardService
{
    Task<CreditCard> GetById(int id);

    Task<bool> HandleCustomerCreatedAsync(CustomerCreated customerCreatedEvent, CancellationToken stoppingToken);
}
