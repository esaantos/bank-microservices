namespace CreditsProposal.Core.Events;
public class CustomerCreated
{
    public CustomerCreated(int id, string fullName, string email, decimal income)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        Income = income;
    }

    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public decimal Income { get; set; }
    
}
