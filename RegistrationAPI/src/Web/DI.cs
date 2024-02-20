namespace RegistrationAPI.Web;

public static class DI
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }
}
