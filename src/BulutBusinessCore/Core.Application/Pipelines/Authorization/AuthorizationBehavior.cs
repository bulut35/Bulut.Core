using BulutBusinessCore.Core.CrossCuttingConcerns.Types;
using BulutBusinessCore.Core.Security.Constants;
using BulutBusinessCore.Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BulutBusinessCore.Core.Application.Pipelines.Authorization;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ISecuredRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (!_httpContextAccessor.HttpContext.User.Claims.Any())
            throw new AuthorizationException("You are not authenticated.");

        if (request.Roles.Any())
        {
            ICollection<string>? userRoleClaims = _httpContextAccessor.HttpContext.User.GetRoleClaims() ?? [];
            bool isMatchedAUserRoleClaimWithRequestRoles = userRoleClaims.Any(userRoleClaim =>
                userRoleClaim == GeneralOperationClaims.Admin || request.Roles.Contains(userRoleClaim)
            );
            if (!isMatchedAUserRoleClaimWithRequestRoles)
                throw new AuthorizationException("You are not authorized.");
        }

        TResponse response = await next();
        return response;
    }
}
