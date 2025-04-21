using BulutBusinessCore.Core.Persistence.DbMigrationApplier;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BulutBusinessCore.Core.Persistence;

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
