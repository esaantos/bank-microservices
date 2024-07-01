using Customers.Core.Events;
using Customers.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Customers.Application.EventHandlers;

public class CreditProposalEventHandler : INotificationHandler<CreditProposalCreatedEvent>
{
    private readonly ILogger<CreditProposalEventHandler> _logger;
    private readonly ICustomersRepository _customerRepository;

    public CreditProposalEventHandler(ILogger<CreditProposalEventHandler> logger, ICustomersRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }

    public async Task Handle(CreditProposalCreatedEvent notification, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(notification.CustomerId);
        if(customer != null)
        {            
            customer.AddCreditProposal(notification.CreditValue);

            await _customerRepository.UpdateCreditProposalsAsync(customer);
            _logger.LogInformation("Customer's credit proposal updated successfully.");
        }
        else
            _logger.LogWarning($"Customer {notification.CustomerId} was not found and actions related to the event were not executed!");
    }
}
