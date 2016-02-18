using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleArch.Repository.Common
{
    public interface IUnitOfWork : IDisposable
    {

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        int Commit();

        Task<int> CommitAsync();

        Task<int> CommitAsync(CancellationToken cancellationToken);
    }
}
