using CreditCards.Core.Entities;
using CreditCards.Core.Events;
using CreditCards.Core.Repositories;
using CreditCards.Infrastructure.MessageBus;
using Microsoft.Extensions.Logging;

namespace CreditCards.Application.Services;
public class CreditCardService : ICreditCardService
{
    private readonly ILogger<CreditCardService> _logger;
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly IEventPublisher _eventPublisher;
    public CreditCardService(ICreditCardRepository creditCardRepository, ILogger<CreditCardService> logger, IEventPublisher eventPublisher)
    {
        _creditCardRepository = creditCardRepository;
        _logger = logger;
        _eventPublisher = eventPublisher;
    }
    public async Task<CreditCard> GetById(int id)
    {
        return await _creditCardRepository.GetByIdAsync(id);
    }

    public async Task<bool> HandleCustomerCreatedAsync(CustomerCreated customerCreatedEvent, CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Creating Credit card...");

            var creditCards = CreditCard.GenerateCreditCards(customerCreatedEvent.Id, customerCreatedEvent.FullName, customerCreatedEvent.Income);

            await _creditCardRepository.AddAsync(creditCards);

            var creditCardCreatedEvent = new CreditCardCreatedEvent(creditCards.Select(i => i.Id).ToList(), customerCreatedEvent.Id);

            await _eventPublisher.Publish(creditCardCreatedEvent);

            _logger.LogInformation($"Credit card created successfully for CustomerId: {customerCreatedEvent.Id}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating credit card for CustomerId: {CustomerId}", customerCreatedEvent.Id);

            return false;
        }

    }
}
