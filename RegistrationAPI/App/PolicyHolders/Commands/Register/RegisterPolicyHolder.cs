using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
    private readonly TimeProvider _timeProvider;

    public RegisterPolicyHolderCommandValidator(IAppDbContext ctx, TimeProvider timeProvider)
    {        
        _ctx = ctx;
        _timeProvider = timeProvider;

        RuleFor(ph => ph.FirstName).NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(ph => ph.LastName).NotEmpty().MinimumLength(3).MaximumLength(50);

        // There's a way to have this generated properly - rather than a literal string! Look up if time.
        RuleFor(ph => ph.PolicyReferenceNumber).Length(9).Matches("[A-Z]{2}-\\d{6}");
        RuleFor(ph => ph.PolicyReferenceNumber).MustAsync(BeUnique).WithMessage("'{PropertyName}' must be unique.").WithErrorCode("Unique");

        // Enforce either Email or DOB
        When(ph => !ph.DateOfBirth.HasValue, 
            () => RuleFor(ph => ph.PolicyHoldersEmail)
            .NotEmpty()
            //4+ AN - @ - 2+ AN - .com/.co.uk
            .Matches("[A-Za-z0-9]{4,}@([A-Za-z0-9]{2,}\\.)?(com|co\\.uk)"));
        
        When(ph => string.IsNullOrEmpty(ph.PolicyHoldersEmail), 
            () => RuleFor(ph => ph.DateOfBirth)
            .NotNull()
            .Must(ph => BeAtLeastEighteen(ph!.Value)));                
    }

    public async Task<bool> BeUnique(string referenceNumber, CancellationToken ct) => await _ctx.PolicyHolders.AllAsync(u => u.PolicyReferenceNumber != referenceNumber, ct);

    public bool BeAtLeastEighteen(DateOnly dob) => dob.AddYears(18) <= DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime);
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
