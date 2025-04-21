using BulutBusinessCore.Core.Persistence.Dynamic;
using BulutBusinessCore.Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using BulutBusinessCore.Core.Application.Dtos;

namespace BulutBusinessCore.Core.Persistence.Repositories;
public interface IViewRepository<TEntityView> : IQuery<TEntityView>
    where TEntityView : class, IDto
{
    TEntityView? Get(
        Expression<Func<TEntityView, bool>> predicate,
        Func<IQueryable<TEntityView>, IIncludableQueryable<TEntityView, object>>? include = null
    );

    IPaginate<TEntityView> GetList(
        Expression<Func<TEntityView, bool>>? predicate = null,
        Func<IQueryable<TEntityView>, IOrderedQueryable<TEntityView>>? orderBy = null,
        Func<IQueryable<TEntityView>, IIncludableQueryable<TEntityView, object>>? include = null,
        int index = 0,
        int size = 10
    );

    IPaginate<TEntityView> GetListByDynamic(
        DynamicQuery dynamic,
        Expression<Func<TEntityView, bool>>? predicate = null,
        Func<IQueryable<TEntityView>, IIncludableQueryable<TEntityView, object>>? include = null,
        int index = 0,
        int size = 10
    );
}
