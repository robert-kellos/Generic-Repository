using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using SampleArch.Modules;
using SampleArch.Repository.Common;

namespace SampleArch
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //// Web API configuration and services
            //// Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            var builder = new ContainerBuilder();

            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EfModule());

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            //builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            //builder.RegisterType<RulesProcessor>().As<IRulesProcessor>().InstancePerRequest();

            //  builder.RegisterAssemblyTypes(typeof([TopLevelRequiredObject; ex: ServerRepository]).Assembly)
            //  .Where(t => t.Name.EndsWith("Repository"))
            //  .AsImplementedInterfaces().InstancePerRequest();

            //  builder.RegisterAssemblyTypes(typeof([TopLevelRequiredObject; ex: ServerService]).Assembly)
            //.Where(t => t.Name.EndsWith("Service"))
            //.AsImplementedInterfaces().InstancePerRequest();

            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
