namespace Customers.Core.Events;

public class CustomerCreated : IDomainEvent
{
    public CustomerCreated(int id, string fullName, string email, decimal income)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        Income = income;
    }

    public int Id { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public decimal Income { get; private set; }


}