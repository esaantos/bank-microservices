using Customers.Core.Entities;
using Customers.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Customers.Infrastructure.Persistence;

public class CustomerContext : DbContext
{
    public CustomerContext(DbContextOptions<CustomerContext> options): base(options)
    {            
    }
    public DbSet<Customer> Customers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerConfigurations).Assembly);
    }

}
