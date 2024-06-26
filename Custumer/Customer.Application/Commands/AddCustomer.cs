using MediatR;

namespace Customers.Application.Commands;

public class AddCustomers : IRequest<int>
{
    public string FullName { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public decimal Income { get; set; }
}
