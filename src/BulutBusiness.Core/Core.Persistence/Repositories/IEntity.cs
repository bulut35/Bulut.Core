namespace BulutBusiness.Core.Core.Persistence.Repositories;

public interface IEntity<T>
{
    T Id { get; set; }
}
