using CreditsProposal.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditsProposal.Infrastructure.Persistence.Configurations;

public class CreditProposalConfiguration : IEntityTypeConfiguration<CreditProposal>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CreditProposal> builder)
    {
        builder
            .HasKey(cc => cc.Id);

        builder
            .Property(cc => cc.CreditValue)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder
            .Property(cc => cc.CustomerId)
            .IsRequired();
    }
}
