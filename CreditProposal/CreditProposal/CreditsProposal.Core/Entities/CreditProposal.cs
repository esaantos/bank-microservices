namespace CreditsProposal.Core.Entities;

public class CreditProposal
{
    public CreditProposal( int customerId, decimal creditValue)
    {
        CustomerId = customerId;
        CreditValue = creditValue;
    }

    public int Id { get; private set; }
    public int CustomerId { get; set; }
    public decimal CreditValue { get; private set; }


    public static decimal GenerateCredit(decimal income)
    {
        return income * 3;
    }
}


