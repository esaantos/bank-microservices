using Customers.Core.Events;

namespace Customers.Core.Entities;

public class Customer : AggregateRoot
{
    public Customer(string fullName, string cpf, string email, DateTime birthDate, decimal income)
    {
        FullName = fullName;
        Cpf = cpf;
        Email = email;
        BirthDate = birthDate;
        Income = income;
    }

    public string FullName { get; private set; }
    public string Cpf { get; private set; }
    public string Email { get; private set; }
    public DateTime BirthDate { get; private set; }
    public decimal Income { get; private set; }
    public int CreditProposalId { get; private set; }
    public List<int> CreditCardIds { get; private set; } = new List<int>();

    public static Customer Create(string fullName, string cpf, string email, DateTime birthDate, decimal income)
    {
        var customer = new Customer(fullName, cpf, email, birthDate, income);

        customer.AddEvent(new CustomerCreated(customer.Id, customer.FullName, customer.Email, customer.Income));

        return customer;
    }

    public void AddCreditCard(List<int> creditCardId)
    {
        CreditCardIds.AddRange(creditCardId);
    }

    public void AddCreditProposal(int creditProposalId)
    {
        CreditProposalId = creditProposalId;
    }
}
