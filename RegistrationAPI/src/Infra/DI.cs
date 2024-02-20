using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegistrationAPI.App.Interfaces;

namespace Infra;

public static class DI
{    
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);

        var connString = configuration.GetConnectionString("Default");

        services.AddDbContext<AppDbContext>(opts => opts.UseSqlite(connString));
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        return services;
    }

    public static async Task InitialiseDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            await ctx.Database.MigrateAsync();
        }
        catch (Exception)
        {
            // Consider logging or some other handling?
            throw;
        }
    }
}
