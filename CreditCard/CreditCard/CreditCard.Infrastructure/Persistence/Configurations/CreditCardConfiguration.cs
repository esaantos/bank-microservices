using CreditCards.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditCards.Infrastructure.Persistence.Configurations;

public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> builder)
    {
        builder
            .HasKey(cc => cc.Id);

        builder
            .Property(cc => cc.CardNumber)
            .IsRequired()
            .HasMaxLength(16);

        builder
            .Property(cc => cc.CardHolderName)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(cc => cc.ExpirationDate)
            .IsRequired();

        builder
            .Property(cc => cc.CVV)
            .IsRequired()
            .HasMaxLength(4);

        builder
            .Property(cc => cc.CustomerId)
            .IsRequired();
    }
}
