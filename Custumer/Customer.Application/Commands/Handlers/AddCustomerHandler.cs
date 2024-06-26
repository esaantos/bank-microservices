using Customers.Application.Commands;
using Customers.Core.Entities;
using Customers.Core.Repositories;
using Customers.Infrastructure.MessageBus;
using MediatR;

namespace Customers.Application.Commands.Handlers;

public class AddCustomersHandler : IRequestHandler<AddCustomers, int>
{
    private readonly ICustomersRepository _repository;
    private readonly IEventProcessor _eventProcessor;

    public AddCustomersHandler(ICustomersRepository repository, IEventProcessor eventProcessor)
    {
        _repository = repository;
        _eventProcessor = eventProcessor;
    }

    public async Task<int> Handle(AddCustomers request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(request.FullName, request.Cpf, request.Email,
                request.BirthDate, request.Income);

        await _repository.AddAsync(customer);

        _eventProcessor.Process(customer.Events);

        return customer.Id;
    }
}
