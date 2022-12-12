using BCCP.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BCCP.Shared.Repositories;

public interface IRepositoryBase<TEntity>
where TEntity : EntityBase
{
    /// <summary>
    /// This method is responsible for getting all the registers from the database based on the Generic Entity <T>.
    /// </summary>
    /// <returns>IReadOnlyList<TEntity></returns>
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    
    /// <summary>
    /// This method is responsible for getting all the registers from the database based on the Generic Entity <T> and in the
    /// Predicate parameter.
    /// </summary>
    /// <param name="Predicate">Predicate to filter the results.</param>
    /// <returns>IReadOnlyList<TEntity></returns>
    Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> Predicate);

    /// <summary>
    /// This method is responsible for getting all the registers from the database based on the Generic Entity <T> and in the
    /// Predicate parameter.
    /// </summary>
    /// <param name="Predicate">Predicate to filter the results.</param>
    /// <param name="OrderBy">Ordering the Results.</param>
    /// <param name="IncludeString">Entity to include.</param>
    /// <param name="DisableTracking">Disable the Entities tracking.</param>
    /// <returns>IReadOnlyList<TEntity></returns>
    Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> Predicate,
                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy = null,
                                    string IncludeString = null,
                                    bool DisableTracking = true);

    /// <summary>
    /// This method is responsible for getting all the registers from the database based on the Generic Entity <T> and in the
    /// Predicate parameter.
    /// </summary>
    /// <param name="Predicate">Predicate to filter the results.</param>
    /// <param name="OrderBy">Ordering the Results.</param>
    /// <param name="Includes">Entities to include.</param>
    /// <param name="DisableTracking">Disable the Entities tracking.</param>
    /// <returns>IReadOnlyList<TEntity></returns>
    Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> Predicate,
                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy = null,
                                    List<Expression<Func<TEntity, object>>> Includes = null,
                                    bool DisableTracking = true);

    /// <summary>
    /// This method is responsible for getting an specific register (by identity) from the database based on the Generic Entity <T> and in the
    /// Id parameter.
    /// </summary>
    /// <param name="Id">Predicate to filter the results.</param>
    /// <returns>TEntity</returns>
    Task<TEntity> GetByIdAsync(string Id);

    /// <summary>
    /// This method is responsible for adding the Entity to the context, add identity and audit data and Save the changes in the database.
    /// </summary>
    /// <param name="Entity">TEntity to store.</param>
    /// <returns>TEntity</returns>
    Task<TEntity> AddAsync(TEntity Entity);

    /// <summary>
    /// This method is responsible for updating the Entity to the context add audit data and Save the changes in the database.
    /// </summary>
    /// <param name="Entity">TEntity to store.</param>
    Task UpdateAsync(TEntity Entity);

    /// <summary>
    /// This method is responsible for deleting the Entity from the context and Save the changes in the database.
    /// </summary>
    /// <param name="Entity">TEntity to update.</param>
    Task DeleteAsync(TEntity Entity);

    /// <summary>
    /// This method is responsible for checking if theres any Entity on the database with the specific Id.
    /// </summary>
    /// <param name="Id">Id to check.</param>
    /// <returns>Exists or Not</returns>
    Task<bool> Exists(string Id);
}
