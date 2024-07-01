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
    public decimal CreditProposalValue { get; private set; }
    public ICollection<string> CreditCardNumbers { get; private set; } = new List<string>();

    public static Customer Create(string fullName, string cpf, string email, DateTime birthDate, decimal income)
    {
        var customer = new Customer(fullName, cpf, email, birthDate, income);

        return customer;
    }

    public void CreateEvent(Customer customer)
    {
        customer.AddEvent(new CustomerCreated(customer.Id, customer.FullName, customer.Email, customer.Income));
    }

    public void AddCreditCard(string creditCardNumber)
    {
        if(!CreditCardNumbers.Contains(creditCardNumber))
            CreditCardNumbers.Add(creditCardNumber);
    }

    public void AddCreditProposal(decimal creditProposalValue)
    {
        if (CreditProposalValue == 0)
            CreditProposalValue = creditProposalValue;
    }
}
