using System.Reflection;
using Autofac;

namespace WebApiPagingAngularClient.Modules
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("SampleArch.Repository"))
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();
        }
    }
}