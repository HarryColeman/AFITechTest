using RegistrationAPI.Domain.Entities.Common;

namespace RegistrationAPI.Domain.Entities;
public class PolicyHolder : BaseEntity
{
    public int OnlineCustomerId => Id;
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PolicyReferenceNumber { get; set; }
    public string? PolicyHoldersEmail { get; set; }
    public DateOnly? DateOfBirth { get; set; }

}
