using BulutBusiness.Core.Persistence.Repositories;

namespace BulutBusiness.Core.Security.Entities;

public class OperationClaim<TId> : Entity<TId>
{
    public string OperationClaimName { get; set; }

    public OperationClaim()
    {
        OperationClaimName = string.Empty;
    }

    public OperationClaim(string operationClaimName)
    {
        OperationClaimName = operationClaimName;
    }

    public OperationClaim(TId id, string operationClaimName)
        : base(id)
    {
        OperationClaimName = operationClaimName;
    }
}
