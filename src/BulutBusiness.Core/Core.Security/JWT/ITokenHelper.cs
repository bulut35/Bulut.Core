using BulutBusiness.Core.Core.Security.Entities;

namespace BulutBusiness.Core.Core.Security.JWT;

public interface ITokenHelper<TUserId, TOperationClaimId, TRefreshTokenId>
{
    public AccessToken CreateToken(User<TUserId> user, IList<OperationClaim<TOperationClaimId>> operationClaims);

    public RefreshToken<TRefreshTokenId, TUserId> CreateRefreshToken(User<TUserId> user, string ipAddress);
}
