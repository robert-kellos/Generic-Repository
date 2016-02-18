using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using SampleArch.Service.Common;
using SampleArch.Model.Common;

// ReSharper disable InconsistentNaming

namespace WebApiPagingAngularClient.Controllers.Base
{
    public abstract class BaseApiController<TEntity> : ApiController, IEntityService<TEntity>, IEntity<long> where TEntity : BaseEntity
    {
        //private readonly CancellationToken _cancellationToken;
        protected IEntityService<TEntity> _service;
        
        public BaseApiController(IEntityService<TEntity> service)
        {
            _service = service;
            //_cancellationToken = new CancellationToken();
        }
        
        public long Id { get { return ((IEntity<long>)_service).Id; } set{ ((IEntity<long>)_service).Id = value; } }


        public int Count => _service.Count;


        public virtual TEntity Add(TEntity entity)
        {
            return _service.Add(entity);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return await _service.AddAsync(entity, cancellationToken);
        }

        public virtual void AddMany(params TEntity[] items)
        {
            _service.AddMany(items);
        }

        public virtual async Task AddManyAsync(CancellationToken cancellationToken, params TEntity[] items)
        {
            await _service.AddManyAsync(cancellationToken, items);
        }

        public virtual TEntity Delete(TEntity entity)
        {
            return _service.Delete(entity);
        }

        public virtual async Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return await _service.DeleteAsync(entity, cancellationToken);
        }

        public virtual IQueryable<TEntity> Filter<TKey>(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 50)
        {
            return _service.Filter<TEntity>(filter, out total, index, size);
        }

        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _service.FindBy(predicate);
        }

        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> include)
        {
            return _service.FindBy(predicate, include);
        }

        public virtual async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> @where, CancellationToken cancellationToken)
        {
            return await _service.FindByAsync(@where, cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> include)
        {
            return await _service.FindByAsync(@where, include);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _service.GetAll();
        }

        public virtual IList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            return _service.GetAll(navigationProperties);
        }

        public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(cancellationToken);
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(id, cancellationToken);
        }

        public virtual IList<TEntity> GetList(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            return _service.GetList(where, navigationProperties);
        }

        public virtual Task<List<TEntity>> GetListAsync(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            return _service.GetListAsync(where, navigationProperties);
        }

        public virtual TEntity GetSingle(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            return _service.GetSingle(where, navigationProperties);
        }

        public virtual void RemoveMany(params TEntity[] items)
        {
            _service.RemoveMany(items);
        }

        public virtual Task RemoveManyAsync(CancellationToken cancellationToken, params TEntity[] items)
        {
            return _service.RemoveManyAsync(cancellationToken, items);
        }

        public virtual int Save()
        {
            return _service.Save();
        }

        public virtual async Task<int?> SaveAsync(CancellationToken cancellationToken)
        {
            return await _service.SaveAsync(cancellationToken);
        }

        public virtual void Update(TEntity entity)
        {
            _service.Update(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(entity, cancellationToken);
        }

        public virtual void UpdateMany(params TEntity[] items)
        {
            _service.UpdateMany(items);
        }

        public virtual async Task UpdateManyAsync(CancellationToken cancellationToken, CancellationToken items)
        {
            await _service.UpdateManyAsync(cancellationToken, items);
        }
    }
}