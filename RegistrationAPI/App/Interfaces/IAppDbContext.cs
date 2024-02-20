using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Domain.Entities;

namespace RegistrationAPI.App.Interfaces;
public interface IAppDbContext
{
    DbSet<PolicyHolder> PolicyHolders { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}
