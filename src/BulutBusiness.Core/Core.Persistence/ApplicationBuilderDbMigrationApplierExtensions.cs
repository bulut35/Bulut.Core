using BulutBusiness.Core.Core.Persistence.DbMigrationApplier;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BulutBusiness.Core.Core.Persistence;

public static class ApplicationBuilderDbMigrationApplierExtensions
{
    public static IApplicationBuilder UseDbMigrationApplier(this IApplicationBuilder app)
    {
        IEnumerable<IDbMigrationApplierService> migrationCreatorServices =
            app.ApplicationServices.GetServices<IDbMigrationApplierService>();
        foreach (IDbMigrationApplierService service in migrationCreatorServices)
            service.Initialize();

        return app;
    }
}
