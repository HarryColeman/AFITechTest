using RegistrationAPI.App.PolicyHolders.Commands.Register;

namespace RegistrationAPI.Web.Endpoints;

public class PolicyHolders
{
    public void Map(WebApplication app)
    {
        app.MapPost(
            "/register",
            (RegisterPolicyHolderCommand command, RegisterPolicyHolderCommandHandler handler, CancellationToken ct)
            => handler.Handle(command, ct))
            .WithName("RegisterPolicyHolder")
            .WithOpenApi();
    }
}
