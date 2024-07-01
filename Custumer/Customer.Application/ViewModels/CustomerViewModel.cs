using Customers.Core.Entities;

namespace Customers.Application.ViewModels;

public class CustomerViewModel
{
    public CustomerViewModel(int id, string fullName, string email, decimal income, decimal creditProposal, List<string> creditCard)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        Income = income;
        CreditProposal = creditProposal;
        CreditCards = creditCard;
    }

    public int Id { get;  set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public decimal Income { get; set; }
    public decimal CreditProposal { get; set; }
    public List<string> CreditCards { get; set; } = new List<string>();
}
