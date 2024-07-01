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
    private readonly IEventProcessor _eventProcessor;

    public CreditProposalService(ILogger<CreditProposalService> logger, ICreditProposalRepository creditProposalRepository, IEventProcessor eventProcessor)
    {
        _logger = logger;
        _creditProposalRepository = creditProposalRepository;
        _eventProcessor = eventProcessor;
    }

    public async Task<CreditProposal> GetById(int id)
    {
        return await _creditProposalRepository.GetByIdAsync(id);
    }

    public async Task HandlerProposalCreateAsync(CustomerCreated customerCreated, CancellationToken stoppingToken)
    {

        _logger.LogInformation("Creating Credit proposal...");

        var existingCredit = await _creditProposalRepository.GetCreditCardsByCustomerIdAsync(customerCreated.Id);
        if (existingCredit != null)
        {
            _logger.LogInformation($"Credit cards already exist for CustomerId: {customerCreated.Id}. Skipping creation.");
            _eventProcessor.Process(existingCredit.Events);
        }
        else
        {
            var creditValue = CreditProposal.GenerateCredit(customerCreated.Income);

            var creditProposal = new CreditProposal(customerCreated.Id, creditValue);

            await _creditProposalRepository.AddAsync(creditProposal);
            _eventProcessor.Process(creditProposal.Events);
        }

        _logger.LogInformation($"Credit proposal created successfully for CustomerId: {customerCreated.Id}");
    }
}
