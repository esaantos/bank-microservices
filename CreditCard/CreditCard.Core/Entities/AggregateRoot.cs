using CreditCards.Core.Events;

namespace CreditCards.Core.Entities;

public abstract class AggregateRoot : IEntityBase
{
    private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
    public int Id { get; protected set;  }
    public IEnumerable<IDomainEvent> Events => _events;

    protected void AddEvent(IDomainEvent @event)
    {
        _events.Add(@event);
    }
}
