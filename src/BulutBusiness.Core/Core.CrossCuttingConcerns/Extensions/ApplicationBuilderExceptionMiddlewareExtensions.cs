using BulutBusiness.Core.Core.CrossCuttingConcerns.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BulutBusiness.Core.Core.CrossCuttingConcerns.Extensions;

public static class ApplicationBuilderExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
