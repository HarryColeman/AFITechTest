using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using RegistrationAPI.App.Interfaces;
using RegistrationAPI.App.PolicyHolders.Commands.Register;
using RegistrationAPI.Domain.Entities;

namespace RegistrationAPITests;

public class RegisterPolicyHolderTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ShouldThrowIfNotValid()
    {
        var validator = new RegisterPolicyHolderCommandValidator(MockAppDbContext().Object, MockTimeProvider().Object);
        var handler = new RegisterPolicyHolderCommandHandler(MockAppDbContext().Object, validator);
        var cmd = new RegisterPolicyHolderCommand { FirstName = "", LastName = "", PolicyReferenceNumber = "" };

        async Task ActionUnderTest() => await handler.Handle(cmd, CancellationToken.None);

        Assert.ThrowsAsync<ValidationException>(ActionUnderTest);
    }

    private static Mock<IAppDbContext> MockAppDbContext()
    {

        // Okay, https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking#testing-with-async-queries
        // It's a little more involved to mock async queries :D
        // Maybe would reconsider and setup an in-memory DB test approach instead.
        var data = new List<PolicyHolder>
        {
            new() { Id = 1, FirstName = "H", LastName = "C", PolicyReferenceNumber = "AA-000001", PolicyHoldersEmail = "hhcc@gmail.com" }
        }
        .AsQueryable();

        var mockDbCtx = new Mock<IAppDbContext>();
        var mockPolicyHolders = new Mock<DbSet<PolicyHolder>>();

        mockPolicyHolders.As<IQueryable<PolicyHolder>>().Setup(m => m.Provider).Returns(data.Provider);
        mockPolicyHolders.As<IQueryable<PolicyHolder>>().Setup(m => m.Expression).Returns(data.Expression);
        mockPolicyHolders.As<IQueryable<PolicyHolder>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockPolicyHolders.As<IQueryable<PolicyHolder>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

        mockDbCtx.Setup(ctx => ctx.PolicyHolders).Returns(mockPolicyHolders.Object);

        return mockDbCtx;
    }

    private static Mock<TimeProvider> MockTimeProvider()
    {
        var mockTP = new Mock<TimeProvider>();

        mockTP.Setup(tp => tp.GetUtcNow()).Returns(new DateTimeOffset(2024, 2, 10, 10, 0, 0, TimeSpan.Zero));

        return mockTP;
    }
}

