using BulutBusiness.Core.Application.Dtos;
using BulutBusiness.Core.Persistence.Dynamic;
using BulutBusiness.Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BulutBusiness.Core.Persistence.Repositories;

public class EfViewRepositoryBase<TEntityView, TContext>
    : IAsyncViewRepository<TEntityView>, IViewRepository<TEntityView>
    where TEntityView : class, IDto
    where TContext : DbContext
{
    protected readonly TContext Context;

    public EfViewRepositoryBase(TContext context)
    {
        Context = context;
    }

    public TEntityView? Get(Expression<Func<TEntityView, bool>> predicate, Func<IQueryable<TEntityView>, IIncludableQueryable<TEntityView, object>>? include = null)
    {
        IQueryable<TEntityView> queryable = Query();
        if (include != null)
            queryable = include(queryable);
        return queryable.FirstOrDefault(predicate);
    }

    public async Task<TEntityView?> GetAsync(Expression<Func<TEntityView, bool>> predicate, Func<IQueryable<TEntityView>, IIncludableQueryable<TEntityView, object>>? include = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntityView> queryable = Query();
        if (include != null)
            queryable = include(queryable);
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public IPaginate<TEntityView> GetList(Expression<Func<TEntityView, bool>>? predicate = null, Func<IQueryable<TEntityView>, IOrderedQueryable<TEntityView>>? orderBy = null, Func<IQueryable<TEntityView>, IIncludableQueryable<TEntityView, object>>? include = null, int index = 0, int size = 10)
    {
        IQueryable<TEntityView> queryable = Query();
        if (include != null)
            queryable = include(queryable);
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return orderBy(queryable).ToPaginate(index, size);
        return queryable.ToPaginate(index, size);
    }

    public async Task<IPaginate<TEntityView>> GetListAsync(Expression<Func<TEntityView, bool>>? predicate = null, Func<IQueryable<TEntityView>, IOrderedQueryable<TEntityView>>? orderBy = null, Func<IQueryable<TEntityView>, IIncludableQueryable<TEntityView, object>>? include = null, int index = 0, int size = 10, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntityView> queryable = Query();
        if (include != null)
            queryable = include(queryable);
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToPaginateAsync(index, size, from: 0, cancellationToken);
        return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
    }

    public IPaginate<TEntityView> GetListByDynamic(DynamicQuery dynamic, Expression<Func<TEntityView, bool>>? predicate = null, Func<IQueryable<TEntityView>, IIncludableQueryable<TEntityView, object>>? include = null, int index = 0, int size = 10)
    {
        IQueryable<TEntityView> queryable = Query().ToDynamic(dynamic);
        if (include != null)
            queryable = include(queryable);
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return queryable.ToPaginate(index, size);
    }

    public async Task<IPaginate<TEntityView>> GetListByDynamicAsync(DynamicQuery dynamic, Expression<Func<TEntityView, bool>>? predicate = null, Func<IQueryable<TEntityView>, IIncludableQueryable<TEntityView, object>>? include = null, int index = 0, int size = 10, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntityView> queryable = Query().ToDynamic(dynamic);
        if (include != null)
            queryable = include(queryable);
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
    }

    public IQueryable<TEntityView> Query()
    {
        return Context.Set<TEntityView>();
    }
}
