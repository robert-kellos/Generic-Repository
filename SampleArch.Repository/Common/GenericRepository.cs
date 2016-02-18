using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using log4net.Config;
using SampleArch.Logging;
using SampleArch.Model.Common;
using SampleArch.Utilities;
// ReSharper disable InconsistentNaming

namespace SampleArch.Repository.Common
{
    /// <summary>
    /// GenericRepository of TEntity
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="SampleArch.Repository.Common.IGenericRepository{TEntity}" />
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : BaseEntity
    {
        /// <summary>
        /// The Dbset
        /// </summary>
        protected readonly IDbSet<TEntity> _currentDbSet;

        /// <summary>
        /// The DbContext
        /// </summary>
        protected DbContext _currentDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity}" /> class.
        /// </summary>
        protected GenericRepository()
        {
            XmlConfigurator.Configure();

            Audit.Log.Info("GenericRepository() :: initialized in constructor");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity}" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        protected GenericRepository(DbContext context)
        {
            _currentDbContext = context;
            AppUtility.ValidateContext(_currentDbContext);

            try
            {
                _currentDbSet = _currentDbContext.Set<TEntity>();
                AppUtility.ValidateDbSet(_currentDbSet);
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }
        }

        /// <summary>
        /// Count of current db table set
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        int IGenericRepository<TEntity>.Count => _currentDbSet?.Count() ?? -1;


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
            var skipCount = index*size;

            var resetSet = filter != null
                ? _currentDbSet.Where(filter).AsQueryable()
                : _currentDbSet.AsQueryable();

            resetSet = skipCount == 0
                ? resetSet.Take(size)
                : resetSet.Skip(skipCount).Take(size);

            total = resetSet.Count();

            return resetSet.AsQueryable();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            var result = default(IEnumerable<TEntity>);

            AppUtility.ValidateDbSet(_currentDbSet);

            try
            {
                result = _currentDbSet.AsEnumerable();
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the list asynchronous.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="navigationProperties">The navigation properties.</param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetListAsync(Func<TEntity, bool> @where,
            params Expression<Func<TEntity, object>>[] @navigationProperties)
        {
            List<TEntity> list = null;

            AppUtility.ValidateContext(_currentDbContext);

            try
            {
                IQueryable<TEntity> dbQuery = _currentDbContext.Set<TEntity>();

                if (navigationProperties != null)
                {
                    //Apply eager loading
                    dbQuery = navigationProperties.Aggregate(
                        dbQuery,
                        (current, navigationProperty) => current.Include(navigationProperty)
                        );
                }

                if (dbQuery == null) return null;

                list = await dbQuery
                    .AsNoTracking()
                    .Where(@where)
                    .AsQueryable()
                    .ToListAsync().ConfigureAwait(true);
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return list;
        }

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="where">The predicate.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> @where)
        {
            IEnumerable<TEntity> query = null;

            AppUtility.ValidateDbSet(_currentDbSet);

            try
            {
                query = _currentDbSet.Where(where).AsEnumerable();
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

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
            IEnumerable<TEntity> result = null;

            AppUtility.ValidateContext(_currentDbContext);

            try
            {
                var dbQuery = _currentDbContext.Set<TEntity>();
                result = await dbQuery.Where(@where).ToListAsync(cancellationToken).ConfigureAwait(true);
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return result;
        }

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="include">The include.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> @where,
            Expression<Func<TEntity, object>> @include)
        {
            var result = default(IEnumerable<TEntity>);

            AppUtility.ValidateContext(_currentDbContext);

            try
            {
                var dbQuery = _currentDbContext.Set<TEntity>();

                result = dbQuery.Where(@where).Include(include).ToList();
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return result;
        }

        /// <summary>
        /// Finds the by asynchronous.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="include">The include.</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> @where,
            Expression<Func<TEntity, object>> @include)
        {
            IEnumerable<TEntity> result = null;

            AppUtility.ValidateContext(_currentDbContext);

            try
            {
                var dbQuery = _currentDbContext.Set<TEntity>();
                result = await dbQuery.Where(@where).Include(include).ToListAsync().ConfigureAwait(true);
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return result;
        }

        /// <summary>
        /// Removes the many asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task RemoveManyAsync(CancellationToken cancellationToken, params TEntity[] items)
        {
            AppUtility.ValidateDbSet(_currentDbSet);

            if (items == null) return;

            try
            {
                foreach (var item in items)
                {
                    _currentDbContext.Entry(item).State = EntityState.Deleted;
                }
                await _currentDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual TEntity Add(TEntity entity)
        {
            var result = default(TEntity);

            AppUtility.ValidateDbSet(_currentDbSet);
            AppUtility.ValidateEntity(entity);

            try
            {
                var added = _currentDbSet.Add(entity);

                _currentDbContext.Entry(entity).State = EntityState.Added;
                //_currentDbContext.SaveChangesAsync();

                result = added;
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return result;
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var result = default(TEntity);

            AppUtility.ValidateDbSet(_currentDbSet);
            AppUtility.ValidateEntity(entity);

            try
            {
                await Task.Factory.StartNew(() => 
                { 
                    var added = _currentDbSet.Add(entity);

                    _currentDbContext.Entry(entity).State = EntityState.Added;
                    //await _currentDbContext.SaveChangesAsync(cancellationToken);

                    result = added;
                }, cancellationToken).ConfigureAwait(true);
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return result;
        }

        /// <summary>
        /// Adds the many.
        /// </summary>
        /// <param name="items">The items.</param>
        public virtual void AddMany(params TEntity[] items)
        {
            AppUtility.ValidateContext(_currentDbContext);

            if (items == null) return;

            try
            {
                foreach (var item in items)
                {
                    _currentDbContext.Entry(item).State = EntityState.Added;
                }
                _currentDbContext.SaveChangesAsync();
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }
        }

        /// <summary>
        /// Adds the many asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task AddManyAsync(CancellationToken cancellationToken, params TEntity[] items)
        {
            AppUtility.ValidateContext(_currentDbContext);

            if (items == null) return;

            try
            {
                foreach (var item in items)
                {
                    _currentDbContext.Entry(item).State = EntityState.Added;
                }
                await _currentDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual TEntity Delete(TEntity entity)
        {
            AppUtility.ValidateDbSet(_currentDbSet);
            AppUtility.ValidateEntity(entity);

            _currentDbContext.Entry(entity).State = EntityState.Deleted;

            var result = _currentDbSet.Remove(entity);
            //_currentDbContext.SaveChanges();

            return result;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            AppUtility.ValidateDbSet(_currentDbSet);
            AppUtility.ValidateEntity(entity);

            _currentDbContext.Entry(entity).State = EntityState.Deleted;

            var result = _currentDbSet.Remove(entity);
            await _currentDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return result;
        }

        /// <summary>
        /// Edits the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(TEntity entity)
        {
            AppUtility.ValidateContext(_currentDbContext);
            AppUtility.ValidateEntity(entity);

            _currentDbContext.Entry(entity).State = EntityState.Modified;
            //_currentDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            AppUtility.ValidateContext(_currentDbContext);
            AppUtility.ValidateEntity(entity);

            _currentDbContext.Entry(entity).State = EntityState.Modified;
            //await _currentDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates the many asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task UpdateManyAsync(CancellationToken cancellationToken, params TEntity[] items)
        {
            AppUtility.ValidateContext(_currentDbContext);

            if (items == null) return;

            try
            {
                foreach (var item in items)
                {
                    _currentDbContext.Entry(item).State = EntityState.Modified;
                }
                await _currentDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }
        }

        /// <summary>
        /// Removes the many.
        /// </summary>
        /// <param name="items">The items.</param>
        public virtual void RemoveMany(params TEntity[] items)
        {
            AppUtility.ValidateDbSet(_currentDbSet);

            if (items == null) return;

            try
            {
                foreach (var item in items)
                {
                    _currentDbContext.Entry(item).State = EntityState.Deleted;
                }
                _currentDbContext.SaveChangesAsync();
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }
        }

        /// <summary>
        /// Updates the many.
        /// </summary>
        /// <param name="items">The items.</param>
        public virtual void UpdateMany(params TEntity[] items)
        {
            AppUtility.ValidateContext(_currentDbContext);

            if (items == null) return;

            try
            {
                foreach (var item in items)
                {
                    _currentDbContext.Entry(item).State = EntityState.Modified;
                }
                _currentDbContext.SaveChangesAsync();
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public virtual int Save()
        {
            var result = -1;

            AppUtility.ValidateContext(_currentDbContext);

            try
            {
                result = _currentDbContext.SaveChanges();

                Audit.Log.Info($"Repository Save() :: result: {result}");
            }
            catch (DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors.SelectMany(
                    x => x.ValidationErrors.Select(y => new ValidationResult(y.ErrorMessage, new[] {y.PropertyName})));

                Audit.Log.Error(
                    $"{AppConstant.ErrorMessages.DbEntityValidationExceptionMessage} :: {errors} ",
                    ex);
            }
            catch (DbUpdateException ex)
            {
                var decodedErrors = TryDecodeDbUpdateException(ex);
                //it isn't something we understand 

                Audit.Log.Error(
                    $"{AppConstant.ErrorMessages.DbExceptionMessage} :: {decodedErrors} ", ex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return result;
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public virtual async Task<int?> SaveAsync(CancellationToken cancellationToken)
        {
            var result = -1;

            AppUtility.ValidateContext(_currentDbContext);

            try
            {
                result = await _currentDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

                Audit.Log.Info($"Repository Save() :: result: {result}");
            }
            catch (DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors.SelectMany(
                    x => x.ValidationErrors.Select(y => new ValidationResult(y.ErrorMessage, new[] { y.PropertyName })));

                Audit.Log.Error(
                    $"{AppConstant.ErrorMessages.DbEntityValidationExceptionMessage} :: {errors} ",
                    ex);
            }
            catch (DbUpdateException ex)
            {
                var decodedErrors = TryDecodeDbUpdateException(ex);
                //it isn't something we understand 

                Audit.Log.Error(
                    $"{AppConstant.ErrorMessages.DbExceptionMessage} :: {decodedErrors} ", ex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return result;
        }

        //
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="navigationProperties">The navigation properties.</param>
        /// <returns></returns>
        public virtual IList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] @navigationProperties)
        {
            //usage example: 
            //IGenericDataRepository<Department> repository = new GenericDataRepository<Department>();
            //IList<Department> departments = repository.GetAll(d => d.Employees);

            var list = default(List<TEntity>);

            AppUtility.ValidateContext(_currentDbContext);

            try
            {
                IQueryable<TEntity> dbQuery = _currentDbContext.Set<TEntity>();

                if (navigationProperties != null)
                {
                    //Apply eager loading
                    dbQuery = navigationProperties.Aggregate(
                        dbQuery,
                        (current, navigationProperty) => current.Include(navigationProperty)
                        );
                }

                if (dbQuery != null)
                {
                    list = dbQuery
                        .AsNoTracking()
                        .ToList();
                }
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return list;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            List<TEntity> list = null;

            AppUtility.ValidateContext(_currentDbContext);

            try
            {
                IQueryable<TEntity> dbQuery = _currentDbContext.Set<TEntity>();

                //Apply eager loading, async
                list = await dbQuery
                    .AsNoTracking()
                    .ToListAsync(cancellationToken).ConfigureAwait(true);
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return list;
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="navigationProperties">The navigation properties.</param>
        /// <returns></returns>
        public virtual IList<TEntity> GetList(Func<TEntity, bool> @where,
            params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            List<TEntity> list = null;

            AppUtility.ValidateContext(_currentDbContext);

            try
            {
                IQueryable<TEntity> dbQuery = _currentDbContext.Set<TEntity>();

                if (navigationProperties != null)
                {
                    //Apply eager loading
                    dbQuery = navigationProperties.Aggregate(
                        dbQuery,
                        (current, navigationProperty) => current.Include(navigationProperty)
                        );
                }

                if (dbQuery == null) return null;

                list = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToList();
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return list;
        }

        /// <summary>
        /// Gets the single.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="navigationProperties">The navigation properties.</param>
        /// <returns></returns>
        public virtual TEntity GetSingle(Func<TEntity, bool> @where,
            params Expression<Func<TEntity, object>>[] @navigationProperties)
        {
            TEntity item = null;

            AppUtility.ValidateDbSet(_currentDbSet);

            try
            {
                IQueryable<TEntity> dbQuery = _currentDbContext.Set<TEntity>();

                if (navigationProperties != null)
                {
                    //Apply eager loading
                    dbQuery = navigationProperties.Aggregate(
                        dbQuery,
                        (current, navigationProperty) => current.Include(navigationProperty)
                        );
                }

                if (dbQuery == null) return null;

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

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
            TEntity item = null;

            AppUtility.ValidateContext(_currentDbContext);

            try
            {
                var dbQuery = _currentDbContext.Set<TEntity>();

                item = await dbQuery.FindAsync(cancellationToken, id).ConfigureAwait(true);
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return item;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

            Audit.Log.Info("GenericRepository Dispose :: CurrentDbContext destroyed");
        }

        /// <summary>
        /// This decodes the DbUpdateException. If there are any errors it can
        /// handle then it returns a list of errors. Otherwise it returns null
        /// which means rethrow the error as it has not been handled
        /// </summary>
        /// <param name="dbuex">The dbuex.</param>
        /// <returns>
        /// null if cannot handle errors, otherwise a list of errors
        /// </returns>
        private static IEnumerable<ValidationResult> TryDecodeDbUpdateException(DbUpdateException dbuex)
        {
            var result = new List<ValidationResult>();
            try
            {
                if (!(dbuex.InnerException is UpdateException) ||
                    !(dbuex.InnerException.InnerException is SqlException))
                    return null;

                var sqlException = (SqlException) dbuex.InnerException.InnerException;
                for (var i = 0; i < sqlException.Errors.Count; i++)
                {
                    var errorNum = sqlException.Errors[i].Number;
                    if (!string.IsNullOrWhiteSpace(errorNum.ToString()))
                        result.Add(new ValidationResult(errorNum.ToString()));
                }

                return result.Any() ? result : null;
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }

            return result;
        }

        /// <summary>
        /// Validate Entity, get list of errors
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate()
        {
            var validationErrors = new List<ValidationResult>();

            var ctx = new ValidationContext(this, null, null);
            try
            {
                Validator.TryValidateObject(this, ctx, validationErrors, true);
            }
            catch (ArgumentNullException argumentNullException)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, argumentNullException);
            }

            return validationErrors;
        }

        /// <summary>
        /// Attaches the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Attach(TEntity entity)
        {
            AppUtility.ValidateContext(_currentDbContext);

            try
            {
                var dbset = _currentDbContext.Set<TEntity>();

                dbset.Attach(entity);
            }
            catch (SqlException sqex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.SqlExceptionMessage, sqex);
            }
            catch (DbException dbex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.DbExceptionMessage, dbex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error(AppConstant.ErrorMessages.ExceptionMessage, ex);
            }
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (_currentDbContext == null) return;
            _currentDbContext.Dispose();

            _currentDbContext.Dispose();
            _currentDbContext = null;
        }
    }
}