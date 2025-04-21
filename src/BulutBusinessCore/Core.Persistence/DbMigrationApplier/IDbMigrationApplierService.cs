using Microsoft.EntityFrameworkCore;

namespace BulutBusinessCore.Core.Persistence.DbMigrationApplier;
public interface IDbMigrationApplierService
{
    public void Initialize();
}
public interface IDbMigrationApplierService<TDbContext> : IDbMigrationApplierService
    where TDbContext : DbContext
{ }