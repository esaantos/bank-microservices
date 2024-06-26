using Customers.Application.ViewModels;
using MediatR;

namespace Customers.Application.Queries;
public class GetCustomerByIdQuery : IRequest<CustomerViewModel>
{
    public GetCustomerByIdQuery(int id)
    {
        Id = id;
    }

    public int Id { get; private set; }
}
