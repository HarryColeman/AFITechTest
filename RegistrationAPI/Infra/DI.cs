using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class DI
{
    // Methods requires to wire this up in the WebProj
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        // Get From Web Proj Later
        var connString = configuration.GetConnectionString("Default");

        services.AddDbContext<AppDbContext>(opts => opts.UseSqlite(connString));

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
