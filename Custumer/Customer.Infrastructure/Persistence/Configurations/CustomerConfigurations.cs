using Customers.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customers.Infrastructure.Persistence.Configurations;

public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder
            .HasKey(c => c.Id);

        builder
            .Property(c => c.Id)
            .UseIdentityColumn();

        builder
            .Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(11)
            .IsFixedLength();

        builder
            .HasIndex(c => c.Cpf)
            .IsUnique();

        builder
            .Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(c => c.BirthDate)
            .IsRequired();

        builder
            .Property(c => c.Income)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder
            .Property(c => c.CreditProposalId);

        builder
            .Property(c => c.CreditCardIds)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());

    }
}
