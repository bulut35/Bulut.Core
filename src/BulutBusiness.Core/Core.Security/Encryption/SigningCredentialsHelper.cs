using Microsoft.IdentityModel.Tokens;

namespace BulutBusiness.Core.Core.Security.Encryption;
public static class SigningCredentialsHelper
{
    public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
    {
        return new(securityKey, SecurityAlgorithms.HmacSha512Signature);
    }
}