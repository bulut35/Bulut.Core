using BulutBusiness.Core.Core.Application.Dtos;
using BulutBusiness.Core.Core.Persistence.Dynamic;
using BulutBusiness.Core.Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BulutBusiness.Core.Core.Persistence.Repositories;

public interface IAsyncViewRepository<TEntityView> : IQuery<TEntityView>
    where TEntityView : class, IDto
{
    Task<TEntityView?> GetAsync(
        Expression<Func<TEntityView, bool>> predicate,
        Func<IQueryable<TEntityView>, IIncludableQueryable<TEntityView, object>>? include = null,
        CancellationToken cancellationToken = default
    );

    Task<IPaginate<TEntityView>> GetListAsync(
        Expression<Func<TEntityView, bool>>? predicate = null,
        Func<IQueryable<TEntityView>, IOrderedQueryable<TEntityView>>? orderBy = null,
        Func<IQueryable<TEntityView>, IIncludableQueryable<TEntityView, object>>? include = null,
        int index = 0,
        int size = 10,
        CancellationToken cancellationToken = default
    );

    Task<IPaginate<TEntityView>> GetListByDynamicAsync(
        DynamicQuery dynamic,
        Expression<Func<TEntityView, bool>>? predicate = null,
        Func<IQueryable<TEntityView>, IIncludableQueryable<TEntityView, object>>? include = null,
        int index = 0,
        int size = 10,
        CancellationToken cancellationToken = default
    );
}
