using RegistrationAPI.Web.Endpoints;

namespace RegistrationAPI.Web;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        new PolicyHolders().Map(app);

        return app;
    }
}
