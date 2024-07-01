using CreditCards.Core.Events;

namespace CreditCards.Core.Entities;

public class CreditCard : AggregateRoot
{
    public CreditCard(int customerId, string cardHolderName, string cardNumber)
    {
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        ExpirationDate = DateTime.UtcNow.AddYears(8);
        CVV = new Random().Next(100, 1000).ToString(); ;
        CustomerId = customerId;

        AddEvent(new CreditCardCreatedEvent(CustomerId, CardHolderName, CardNumber, ExpirationDate, CVV));
    }

    public string CardNumber { get; private set; }
    public string CardHolderName { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public string CVV { get; private set; }
    public int CustomerId { get; private set; }

    public static string GenerateCreditCardNumber()
    {
        Random rand = new Random();
        string issuer = GetRandomIssuer(); // Função para escolher um emissor aleatório (Visa, MasterCard, etc.)
        string number = issuer;

        // Preenche os próximos 12 dígitos com números aleatórios
        for (int i = 0; i < 12; i++)
        {
            number += rand.Next(0, 10);
        }

        // Calcula o dígito verificador usando o algoritmo de Luhn
        int[] digits = number.Select(c => Convert.ToInt32(c.ToString())).ToArray();
        int sum = 0;
        bool doubleDigit = false;

        for (int i = digits.Length - 1; i >= 0; i--)
        {
            int digit = digits[i];

            if (doubleDigit)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
            doubleDigit = !doubleDigit;
        }

        int checkDigit = (sum * 9) % 10;

        return number + checkDigit;
    }

    private static string GetRandomIssuer()
    {
        string[] issuers = ["4", "51", "52", "53", "54", "55"]; // Exemplo: Visa, MasterCard range
        Random rand = new Random();
        int index = rand.Next(0, issuers.Length);
        return issuers[index];
    }

    public static List<CreditCard> GenerateCreditCards(int id, string fullName, decimal income)
    {
        var creditCards = new List<CreditCard>();

        if (income >= 5000 && income < 10000)
        {
            creditCards.Add(new CreditCard(id, fullName, CreditCard.GenerateCreditCardNumber()));
            creditCards.Add(new CreditCard(id, fullName, CreditCard.GenerateCreditCardNumber()));
        }
        else if (income >= 10000)
        {
            creditCards.Add(new CreditCard(id, fullName, CreditCard.GenerateCreditCardNumber()));
            creditCards.Add(new CreditCard(id, fullName, CreditCard.GenerateCreditCardNumber()));
            creditCards.Add(new CreditCard(id, fullName, CreditCard.GenerateCreditCardNumber()));
        }
        else
        {
            creditCards.Add(new CreditCard(id, fullName, CreditCard.GenerateCreditCardNumber()));
        }
        return creditCards;
    }

}
