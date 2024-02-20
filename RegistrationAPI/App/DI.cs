using Microsoft.Extensions.DependencyInjection;

namespace App;
public static class DI
{
    // More wiring here from Web when needed.
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services;
    }
}
