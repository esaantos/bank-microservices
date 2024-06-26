
using CreditsProposal.Core.Entities;
using CreditsProposal.Core.Events;
using CreditsProposal.Core.Repositories;
using CreditsProposal.Infrastructure.MessageBus;
using Microsoft.Extensions.Logging;

namespace CreditsProposal.Application.Services;

public class CreditProposalService : ICreditProposalService
{
    private readonly ILogger<CreditProposalService> _logger;
    private readonly ICreditProposalRepository _creditProposalRepository;
    private readonly IEventPublisher _eventPublisher;

    public CreditProposalService(ILogger<CreditProposalService> logger, ICreditProposalRepository creditProposalRepository, IEventPublisher eventPublisher)
    {
        _logger = logger;
        _creditProposalRepository = creditProposalRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<CreditProposal> GetById(int id)
    {
        return await _creditProposalRepository.GetByIdAsync(id);
    }

    public async Task<bool> HandlerProposalCreateAsync(CustomerCreated customerCreated, CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Creating Credit proposal...");

            var creditValue = CreditProposal.GenerateCredit(customerCreated.Income);

            var creditProposal = new CreditProposal(customerCreated.Id, creditValue);

            await _creditProposalRepository.AddAsync(creditProposal);

            var creditProposalEvent = new CreditProposalCreatedEvent(customerCreated.Id, creditProposal.Id);

            await _eventPublisher.Publish(creditProposalEvent);

            _logger.LogInformation($"Credit proposal created successfully for CustomerId: {customerCreated.Id}");

            return true;
        }
        catch (Exception ex)
        {
           _logger.LogError($"Error creating credit card for CustomerId: {customerCreated.Id} | {ex.Message}");
            return false;
        }
    }
}
