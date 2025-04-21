using BulutBusinessCore.Core.Security.EmailAuthenticator;
using BulutBusinessCore.Core.Security.JWT;
using BulutBusinessCore.Core.Security.OtpAuthenticator.OtpNet;
using BulutBusinessCore.Core.Security.OtpAuthenticator;
using Microsoft.Extensions.DependencyInjection;

namespace BulutBusinessCore.Core.Security;
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