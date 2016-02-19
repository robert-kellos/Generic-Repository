using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SampleArch.Logging;
using SampleArch.Model.Common;
using SampleArch.Repository.Common;
using SampleArch.Utilities;

namespace SampleArch.Service.Common
{
    /// <summary>
    /// EntityService
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="SampleArch.Service.Common.IEntityService{TEntity}" />
    public abstract class EntityService<TEntity> : IEntityService<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        private IUnitOfWork _unitOfWork;
        /// <summary>
        /// The _repository
        /// </summary>
        private readonly IGenericRepository<TEntity> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityService{TEntity}" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="repository">The repository.</param>
        protected EntityService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }


        /// <summary>
        /// Gets objects from database with filtering and paging.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        /// <returns></returns>
        public IQueryable<TEntity> Filter<TKey>(Expression<Func<TEntity, bool>> filter,
            out int total, int index = 0, int size = 50)
        {
            return _repository.Filter<TKey>(filter, out total, index, size);
        }

        /// <summary>
        /// Count of current db table set
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count => _repository.Count;

        public static void ClientRequestDownload()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            Task.Run(() =>
            {
                var wc = new WebClient();

                // Create an event handler to receive the result.
                // Check status of WebClient, not external token.
                wc.DownloadStringCompleted += (obj, e) 
                => { Audit.Log.Debug(!e.Cancelled ? "The download has completed:\n" : "The download was canceled."); };

                // Do not initiate download if the external token
                // has already been canceled.
                if (token.IsCancellationRequested) return;
                // Register the callback to a method that can unblock.
                using (token.Register(() => wc.CancelAsync()))
                {
                    Audit.Log.Debug("Starting request\n");
                    wc.DownloadStringAsync(new Uri("http://www.news.google.com"));
                }
                
            }, token);
            
            //cts.Dispose();
        }


        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual TEntity Add(TEntity entity)
        {
            AppUtility.ValidateEntity(entity);

            //calls repository
            var result = _repository.Add(entity);
            _unitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            AppUtility.ValidateEntity(entity);

            //calls repository
            var result = _repository.Add(entity);
            await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(true);

            return result;
        }

        /// <summary>
        /// Adds the many.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddMany(params TEntity[] items)
        {
            //calls repository
            _repository.AddMany(items);
            _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Adds the many asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public async Task AddManyAsync(CancellationToken cancellationToken, params TEntity[] items)
        {
            //calls repository
            _repository.AddMany(items);
            await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(true);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual TEntity Delete(TEntity entity)
        {
            AppUtility.ValidateEntity(entity);

            //calls repository
            var result = _repository.Delete(entity);
            _unitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            AppUtility.ValidateEntity(entity);

            //calls repository
            var result = _repository.Delete(entity);
            await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(true);

            return result;
        }

        /// <summary>
        /// Removes the many.
        /// </summary>
        /// <param name="items">The items.</param>
        public void RemoveMany(params TEntity[] items)
        {
            //calls repository
            _repository.RemoveMany(items);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Removes the many asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public async Task RemoveManyAsync(CancellationToken cancellationToken, params TEntity[] items)
        {
            //calls repository
            _repository.RemoveMany(items);
            await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(true);
        }

        /// <summary>
        /// Edits the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Update(TEntity entity)
        {
            AppUtility.ValidateEntity(entity);

            //calls repository
            _repository.Update(entity);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            AppUtility.ValidateEntity(entity);

            //calls repository
            await _repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(true);
            await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates the many.
        /// </summary>
        /// <param name="items">The items.</param>
        public void UpdateMany(params TEntity[] items)
        {
            //calls repository
            _repository.UpdateMany(items);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Updates the many asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public async Task UpdateManyAsync(CancellationToken cancellationToken, CancellationToken items)
        {
            //calls repository
            await _repository.UpdateManyAsync(items).ConfigureAwait(true);
            await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(true);
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public virtual int Save()
        {
            var result = _unitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public virtual async Task<int?> SaveAsync(CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(true);

            return result;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            var list = _repository.GetAll();

            return list;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="navigationProperties">The navigation properties.</param>
        /// <returns></returns>
        public virtual IList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            //usage example: 
            //IGenericDataRepository<Department> repository = new GenericDataRepository<Department>();
            //IList<Department> departments = repository.GetAll(d => d.Employees);

            var list = _repository.GetAll(navigationProperties).ToList();

            return list;
        }

        /// <summary>
        /// GetAll async
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            var list = await _repository.GetAllAsync(cancellationToken).ConfigureAwait(true);

            return list;
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="navigationProperties">The navigation properties.</param>
        /// <returns></returns>
        public virtual IList<TEntity> GetList(Func<TEntity, bool> where,
            params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            var list = _repository.GetList(@where, navigationProperties).ToList();

            return list;
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="navigationProperties">The navigation properties.</param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetListAsync(Func<TEntity, bool> where,
            params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            var list = await _repository.GetListAsync(@where, navigationProperties).ConfigureAwait(true);

            return list;
        }

        /// <summary>
        /// Gets the single.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="navigationProperties">The navigation properties.</param>
        /// <returns></returns>
        public virtual TEntity GetSingle(Func<TEntity, bool> where,
            params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            var item = _repository.GetSingle(@where, navigationProperties);

            return item;
        }

        /// <summary>
        /// Gets the single asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(true);

            return item;
        }

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> where)
        {
            var query = _repository.FindBy(@where);

            return query;
        }

        /// <summary>
        /// Finds the by asynchronous.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> @where, CancellationToken cancellationToken)
        {
            var query = await _repository.FindByAsync(@where, cancellationToken).ConfigureAwait(true);

            return query;
        }

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="include">The include.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> include)
        {
            var result = _repository.FindBy(@where, include);

            return result;
        }

        /// <summary>
        /// Finds the by asynchronous.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="include">The include.</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> include)
        {
            var result = await _repository.FindByAsync(@where, include).ConfigureAwait(true);

            return result;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="EntityService{TEntity}" /> class.
        /// </summary>
        ~EntityService()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        private void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

            Audit.Log.Info($"Dispose :: CurrentDbContext Entity {typeof (TEntity).DeclaringType} destroyed");
        }


        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (!disposing) return;

            if(_unitOfWork == null) return;
            _unitOfWork.Dispose();

            _unitOfWork = null;

            Dispose();
        }

    }
}
