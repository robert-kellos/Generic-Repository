using System.Collections.Generic;
using SampleArch.Model.Common;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SampleArch.Service.Common
{
    /// <summary>
    /// IEntityService
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="SampleArch.Service.Common.IService" />
    public interface IEntityService<TEntity> : IService
     where TEntity : BaseEntity
    {
        /// <summary>
        /// Count of current db table set
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        int Count { get; }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="navigationProperties">The navigation properties.</param>
        /// <returns></returns>
        IList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] navigationProperties);

        /// <summary>
        /// Gets objects from database with filtering and paging.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        /// <returns></returns>
        IQueryable<TEntity> Filter<TKey>(Expression<Func<TEntity, bool>> filter,
            out int total, int index = 0, int size = 50);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="navigationProperties">The navigation properties.</param>
        /// <returns></returns>
        IList<TEntity> GetList(Func<TEntity, bool> where,
            params Expression<Func<TEntity, object>>[] navigationProperties);

        /// <summary>
        /// Gets the list asynchronous.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="navigationProperties">The navigation properties.</param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Func<TEntity, bool> where,
            params Expression<Func<TEntity, object>>[] navigationProperties);

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Finds the by asynchronous.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="include">The include.</param>
        /// <returns></returns>
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> include);

        /// <summary>
        /// Finds the by asynchronous.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="include">The include.</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> include);

        /// <summary>
        /// Gets the single.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="navigationProperties">The navigation properties.</param>
        /// <returns></returns>
        TEntity GetSingle(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties);

        /// <summary>
        /// Gets the single asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// AddMany
        /// </summary>
        /// <param name="items">The items.</param>
        void AddMany(params TEntity[] items);

        /// <summary>
        /// Adds the many asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        Task AddManyAsync(CancellationToken cancellationToken, params TEntity[] items);

        /// <summary>
        /// UpdateMany
        /// </summary>
        /// <param name="items">The items.</param>
        void UpdateMany(params TEntity[] items);

        /// <summary>
        /// Updates the many asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        Task UpdateManyAsync(CancellationToken cancellationToken, CancellationToken items);

        /// <summary>
        /// RemoveMany
        /// </summary>
        /// <param name="items">The items.</param>
        void RemoveMany(params TEntity[] items);

        /// <summary>
        /// Removes the many asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        Task RemoveManyAsync(CancellationToken cancellationToken, params TEntity[] items);


        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        TEntity Delete(TEntity entity);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Edits the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        int Save();

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<int?> SaveAsync(CancellationToken cancellationToken);
    }
}
