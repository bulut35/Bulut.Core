namespace BulutBusiness.Core.Core.Persistence.Repositories;

public interface IQuery<T>
{
    IQueryable<T> Query();
}
