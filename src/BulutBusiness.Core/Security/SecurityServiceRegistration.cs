using BulutBusiness.Core.Security.EmailAuthenticator;
using BulutBusiness.Core.Security.JWT;
using BulutBusiness.Core.Security.OtpAuthenticator;
using BulutBusiness.Core.Security.OtpAuthenticator.OtpNet;
using Microsoft.Extensions.DependencyInjection;

namespace BulutBusiness.Core.Security;
public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityServices<TUserId, TOperationClaimId, TRefreshTokenId>(
        this IServiceCollection services,
        TokenOptions tokenOptions
    )
    {
        services.AddScoped<
            ITokenHelper<TUserId, TOperationClaimId, TRefreshTokenId>,
            JwtHelper<TUserId, TOperationClaimId, TRefreshTokenId>
        >(_ => new JwtHelper<TUserId, TOperationClaimId, TRefreshTokenId>(tokenOptions));
        services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();

        return services;
    }
}