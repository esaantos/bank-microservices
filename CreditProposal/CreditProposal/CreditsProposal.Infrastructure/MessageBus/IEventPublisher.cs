﻿namespace CreditsProposal.Infrastructure.MessageBus;

public interface IEventPublisher
{
    Task Publish<T>(T @event) where T : class;
}
