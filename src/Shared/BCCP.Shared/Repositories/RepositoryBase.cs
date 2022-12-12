using BCCP.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BCCP.Shared.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : EntityBase
{
    protected readonly DbContext _dbContext;

    public RepositoryBase(DbContext DbContext)
    {
        _dbContext = DbContext ?? throw new ArgumentNullException(nameof(DbContext));
    }

    ///<see cref="IRepositoryBase.GetAllAsync"/>

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    ///<see cref="IRepositoryBase.GetAsync(Expression{Func{TEntity, bool}})"/>
    public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> Predicate)
    {
        return await _dbContext.Set<TEntity>().Where(Predicate).ToListAsync();
    }

    ///<see cref="IRepositoryBase.GetAsync(Expression{Func{TEntity, bool}}, Func{IQueryable{TEntity}, IOrderedQueryable{TEntity}}, string, bool)"/>
    public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> Predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy = null, string IncludeString = null, bool DisableTracking = true)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();
        if (DisableTracking) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(IncludeString)) query = query.Include(IncludeString);

        if (Predicate != null) query = query.Where(Predicate);

        if (OrderBy != null)
            return await OrderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    ///<see cref="IRepositoryBase.GetAsync(Expression{Func{TEntity, bool}}, Func{IQueryable{TEntity}, IOrderedQueryable{TEntity}}, List{Expression{Func{TEntity, object}}}, bool)"/>
    public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> Predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy = null, List<Expression<Func<TEntity, object>>> Includes = null, bool DisableTracking = true)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();
        if (DisableTracking) query = query.AsNoTracking();

        if (Includes != null) query = Includes.Aggregate(query, (current, include) => current.Include(include));

        if (Predicate != null) query = query.Where(Predicate);

        if (OrderBy != null)
            return await OrderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    ///<see cref="IRepositoryBase.GetByIdAsync(string)"/>
    public async Task<TEntity> GetByIdAsync(string Id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(Id);
    }

    ///<see cref="IRepositoryBase.AddAsync(TEntity)"/>
    public async Task<TEntity> AddAsync(TEntity Entity)
    {
        Entity.Id = Guid.NewGuid().ToString();
        var operationDate = DateTime.Now.ToUniversalTime();
        Entity.CreatedDate = operationDate;
        Entity.ModifiedDate = operationDate;

        _dbContext.Set<TEntity>().Add(Entity);
        await _dbContext.SaveChangesAsync();
        return Entity;
    }

    ///<see cref="IRepositoryBase.UpdateAsync(TEntity)"/>
    public async Task UpdateAsync(TEntity Entity)
    {
        Entity.ModifiedDate = DateTime.Now.ToUniversalTime();
        _dbContext.Entry(Entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    ///<see cref="IRepositoryBase.DeleteAsync(TEntity)"/>
    public async Task DeleteAsync(TEntity Entity)
    {
        _dbContext.Set<TEntity>().Remove(Entity);
        await _dbContext.SaveChangesAsync();
    }

    ///<see cref="IRepositoryBase.Exists(string)"/>
    public async Task<bool> Exists(string Id)
    {
        return await _dbContext.Set<TEntity>().AnyAsync(a => a.Id == Id);
    }
}
