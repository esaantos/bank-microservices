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
    private readonly IEventProcessor _eventProcessor;

    public CreditCardService(ICreditCardRepository creditCardRepository, ILogger<CreditCardService> logger, IEventProcessor eventProcessor)
    {
        _logger = logger;
        _creditCardRepository = creditCardRepository;
        _eventProcessor = eventProcessor;
    }
    public async Task<CreditCard> GetById(int id)
    {
        return await _creditCardRepository.GetByIdAsync(id);
    }

    public async Task HandleCustomerCreatedAsync(CustomerCreated customerCreatedEvent, CancellationToken stoppingToken)
    {
        _logger.LogInformation("Creating Credit card...");

        var existingCards = await _creditCardRepository.GetCreditCardsByCustomerIdAsync(customerCreatedEvent.Id);
        if (existingCards.Any())
        {
            _logger.LogInformation($"Credit cards already exist for CustomerId: {customerCreatedEvent.Id}. Skipping creation.");
            foreach (var card in existingCards) {

                _eventProcessor.Process(card.Events);
            }
        }
        else
        {
            var creditCards = CreditCard.GenerateCreditCards(customerCreatedEvent.Id, customerCreatedEvent.FullName, customerCreatedEvent.Income);

            foreach (var creditCard in creditCards) 
            {
                await _creditCardRepository.AddAsync(creditCard);

                _eventProcessor.Process(creditCard.Events);
            }
        }

        _logger.LogInformation($"Credit cards created successfully for CustomerId: {customerCreatedEvent.Id}");
    }
}
