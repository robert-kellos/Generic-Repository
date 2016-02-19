using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace SampleArch.Repository.Common
{
    /// <summary>
    /// The Entity Framework implementation of IUnitOfWork
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The DbContext
        /// </summary>
        private DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        /// <param name="context">The object context</param>
        public UnitOfWork(DbContext context)
        {         

            _dbContext = context;
        }
        
        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit()
        {
            // Save changes with the default options
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            // Save changes with the default options
            return await _dbContext.SaveChangesAsync().ConfigureAwait(true);
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            // Save changes with the default options
            return await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_dbContext == null) return;

            _dbContext.Dispose();
            _dbContext = null;
        }
    }
}
