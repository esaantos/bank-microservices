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

    public async Task Handle(CreditCardCreatedEvent notification, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(notification.CustomerId);
        if(customer != null)
        {
            customer.AddCreditCard(notification.CardNumber);

            await _customerRepository.UpdateCreditCardNumbersAsync(customer);
            _logger.LogInformation("Customer's credit card added successfully.");
        }
        else
            _logger.LogWarning($"Customer {notification.CustomerId} was not found and actions related to the event were not executed!");
    }

}
