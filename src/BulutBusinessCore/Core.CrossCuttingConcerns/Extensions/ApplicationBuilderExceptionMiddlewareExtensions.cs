using BulutBusinessCore.Core.CrossCuttingConcerns.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BulutBusinessCore.Core.CrossCuttingConcerns.Extensions;

public static class ApplicationBuilderExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
