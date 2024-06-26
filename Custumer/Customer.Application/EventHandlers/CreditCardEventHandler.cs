using Customers.Core.Events;
using Customers.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Customers.Application.Events;

public class CreditCardEventHandler : INotificationHandler<CreditCardCreatedEvent>
{
    private readonly ILogger<CreditCardEventHandler> _logger;
    private readonly ICustomersRepository _customerRepository;

    public CreditCardEventHandler(ICustomersRepository customersRepository, ILogger<CreditCardEventHandler> logger)
    {
        _customerRepository = customersRepository;
        _logger = logger;
    }

    public async Task Handle(CreditCardCreatedEvent @event, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(@event.CustomerId);
        if (customer != null)
        {
            customer.AddCreditCard(@event.CreditCardId);
            await _customerRepository.UpdateAsync(customer);
            _logger.LogInformation("Customer's credit card added successfully.");
        }
        _logger.LogWarning($"Customer {@event.CustomerId} was not found and actions related to the event were not executed!");
    }

}
