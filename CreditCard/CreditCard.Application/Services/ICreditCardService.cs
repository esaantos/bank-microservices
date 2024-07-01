using CreditCards.Core.Entities;
using CreditCards.Core.Events;

namespace CreditCards.Application.Services;

public interface ICreditCardService
{
    Task<CreditCard> GetById(int id);

    Task HandleCustomerCreatedAsync(CustomerCreated customerCreatedEvent, CancellationToken stoppingToken);
}
