using Customers.Application.ViewModels;
using Customers.Core.Repositories;
using MediatR;

namespace Customers.Application.Queries.Handler;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerViewModel>
{
    private readonly ICustomersRepository _repository;

    public GetCustomerByIdQueryHandler(ICustomersRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerViewModel> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.Id);

        if (customer is null)
            return null;

        return new CustomerViewModel(customer.Id, customer.FullName, customer.Email, customer.Income, 
            customer.CreditProposalId, customer.CreditCardIds);
    }
}
