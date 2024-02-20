using RegistrationAPI.App.Interfaces;
using RegistrationAPI.App.PolicyHolders.Commands.Register;

namespace RegistraionAPITests;

public class RegisterPolicyHolderCommandHandlerTests
{
    [Fact]
    public void EmptyCommandShouldThrow()
    {        

        var handler = new RegisterPolicyHolderCommandHandler();
    }

    public void SetupHandler()
    {
        var mockCtx = new Mock<IAppDbContext>();
    }
}
