﻿using CreditsProposal.Core.Entities;
using CreditsProposal.Core.Events;

namespace CreditsProposal.Application.Services;

public interface ICreditProposalService
{
    Task<CreditProposal> GetById(int id);

    Task HandlerProposalCreateAsync(CustomerCreated customerCreated, CancellationToken stoppingToken);
}
