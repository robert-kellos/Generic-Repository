using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SampleArch.Model.Common;

namespace SampleArch.Model
{

    public class SampleArchContext : DbContext
    {

        public SampleArchContext()
            : base("Name=SampleArchContext")
        {
            //
            Configuration.LazyLoadingEnabled = false;
        }

        #region List of Entity Sets
        //
        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }
        //
        #endregion List of Entity Sets

        public override int SaveChanges()
        {
            var result = default(int);

            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = entry.Entity as IAuditableEntity;
                if (entity == null) continue;
                if (Thread.CurrentPrincipal == null) continue;

                var identityName ="";
                try
                {
                    identityName = Thread.CurrentPrincipal.Identity.Name;
                }
                catch (SecurityException ex)
                {
                    Trace.TraceError("{0}", ex);
                }

                var now = DateTime.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedBy = identityName;
                    entity.CreatedDate = now;
                }
                else {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;                   
                }

                entity.UpdatedBy = identityName;
                entity.UpdatedDate = now;
            }

            try
            {
                result = base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Trace.TraceError("{0}", ex);
            }
            catch (DbEntityValidationException ex)
            {
                Trace.TraceError("{0}", ex);
            }
            catch (NotSupportedException ex)
            {
                Trace.TraceError("{0}", ex);
            }
            catch (ObjectDisposedException ex)
            {
                Trace.TraceError("{0}", ex);
            }
            catch (InvalidOperationException ex)
            {
                Trace.TraceError("{0}", ex);
            }

            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var result = default(Task<int>);

            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = entry.Entity as IAuditableEntity;
                if (entity == null) continue;
                if (Thread.CurrentPrincipal == null) continue;

                var identityName = "";
                try
                {
                    identityName = Thread.CurrentPrincipal.Identity.Name;
                }
                catch (SecurityException ex)
                {
                    Trace.TraceError("{0}", ex);
                }

                var now = DateTime.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedBy = identityName;
                    entity.CreatedDate = now;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedBy = identityName;
                entity.UpdatedDate = now;
            }

            try
            {
                result = base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                Trace.TraceError("{0}", ex);
            }
            catch (DbEntityValidationException ex)
            {
                Trace.TraceError("{0}", ex);
            }
            catch (NotSupportedException ex)
            {
                Trace.TraceError("{0}", ex);
            }
            catch (ObjectDisposedException ex)
            {
                Trace.TraceError("{0}", ex);
            }
            catch (InvalidOperationException ex)
            {
                Trace.TraceError("{0}", ex);
            }

            return result;
        }
    }    
}
