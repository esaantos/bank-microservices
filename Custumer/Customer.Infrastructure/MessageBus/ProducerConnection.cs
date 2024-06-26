using RabbitMQ.Client;

namespace Customers.Infrastructure.MessageBus;

public class ProducerConnection
{
    public ProducerConnection(IConnection connection)
    {
        Connection = connection;
    }

    public IConnection Connection { get; private set; }
}
