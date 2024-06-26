using CreditCards.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditCards.Infrastructure.Persistence;
public class CreditCardContext : DbContext
{
    public CreditCardContext(DbContextOptions<CreditCardContext> options) : base(options)
    {
            
    }
    public DbSet<CreditCard> CreditCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CreditCardContext).Assembly);
    }
}
