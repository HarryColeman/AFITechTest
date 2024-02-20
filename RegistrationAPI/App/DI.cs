using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RegistrationAPI.App.PolicyHolders.Commands.Register;

namespace App;
public static class DI
{    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient<RegisterPolicyHolderCommandHandler>();

        return services;
    }
}
