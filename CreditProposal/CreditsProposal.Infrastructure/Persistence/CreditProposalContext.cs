using CreditsProposal.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditsProposal.Infrastructure.Persistence;

public class CreditProposalContext : DbContext
{
    public CreditProposalContext(DbContextOptions<CreditProposalContext> options) : base(options)
    {
            
    }

    public DbSet<CreditProposal> CreditProposals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CreditProposalContext).Assembly);
    }
}
