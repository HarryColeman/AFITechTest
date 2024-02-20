using Microsoft.EntityFrameworkCore;
using RegistrationAPI.App.Interfaces;
using RegistrationAPI.Domain.Entities;

namespace Infra;
public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts), IAppDbContext
{
    public DbSet<PolicyHolder> PolicyHolders => Set<PolicyHolder>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<PolicyHolder>().HasKey(ph => ph.Id);
        builder.Entity<PolicyHolder>().Property(ph => ph.FirstName).IsRequired().HasMaxLength(50);
        builder.Entity<PolicyHolder>().Property(ph => ph.LastName).IsRequired().HasMaxLength(50);
        builder.Entity<PolicyHolder>().Property(ph => ph.PolicyReferenceNumber).IsRequired().HasMaxLength(9);

        base.OnModelCreating(builder);
    }
}
