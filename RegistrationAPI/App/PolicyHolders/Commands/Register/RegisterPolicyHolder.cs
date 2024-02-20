using FluentValidation;
using RegistrationAPI.App.Interfaces;
using RegistrationAPI.Domain.Entities;

namespace RegistrationAPI.App.PolicyHolders.Commands.Register;
public record RegisterPolicyHolderCommand
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PolicyReferenceNumber { get; set; }
    public string? PolicyHoldersEmail { get; set; }
    public DateOnly? DateOfBirth { get; set; }
}

public class RegisterPolicyHolderCommandValidator : AbstractValidator<RegisterPolicyHolderCommand>
{
    private readonly IAppDbContext _ctx;

    public RegisterPolicyHolderCommandValidator(IAppDbContext ctx)
    {
        // Inject for any database checks - unique email maybe?
        _ctx = ctx;

        // Basic vali for now, return to this
        RuleFor(v => v.FirstName).NotEmpty();
        RuleFor(v => v.LastName).NotEmpty();
    }
}

public class RegisterPolicyHolderCommandHandler(IAppDbContext ctx, IValidator<RegisterPolicyHolderCommand> validator)
{
    private readonly IAppDbContext _ctx = ctx;
    private readonly IValidator<RegisterPolicyHolderCommand> _validator = validator;

    public async Task<int> Handle(RegisterPolicyHolderCommand command, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(command, ct);

        var entity = new PolicyHolder
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            PolicyReferenceNumber = command.PolicyReferenceNumber,
            PolicyHoldersEmail = command.PolicyHoldersEmail,
            DateOfBirth = command.DateOfBirth
        };

        _ctx.PolicyHolders.Add(entity);

        await _ctx.SaveChangesAsync(ct);

        return entity.Id;
    }
}
