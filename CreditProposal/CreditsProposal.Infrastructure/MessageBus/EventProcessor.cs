using CreditsProposal.Core.Events;
using System.Text;

namespace CreditsProposal.Infrastructure.MessageBus;

public class EventProcessor : IEventProcessor
{
    private readonly IMessageBusClient _messageBusClient;
    private const string Exchange = "credit-proposal-exchange";

    public EventProcessor(IMessageBusClient messageBusClient)
    {
        _messageBusClient = messageBusClient;
    }

    public void Process(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            _messageBusClient.Publish(@event, MapConvention(@event),
                Exchange);
        }
    }

    private string MapConvention(IDomainEvent @event)
    {
        return ToDashCase(@event.GetType().Name);
    }

    public string ToDashCase(string text)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }
        if (text.Length < 2)
        {
            return text;
        }
        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(text[0]));
        for (int i = 1; i < text.Length; ++i)
        {
            char c = text[i];
            if (char.IsUpper(c))
            {
                sb.Append('-');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        Console.WriteLine($"ToDashCase: " + sb.ToString());

        return sb.ToString();
    }
}

public interface IEventProcessor
{
    void Process (IEnumerable<IDomainEvent> events);
}
