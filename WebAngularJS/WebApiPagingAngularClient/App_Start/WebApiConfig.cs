using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json.Serialization;
using SampleArch.Model.Common;
using SampleArch.Repository.Common;
using SampleArch.Service.Common;
using WebApiPagingAngularClient.Modules;

namespace WebApiPagingAngularClient
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services          

            // Use camelCase for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //// Web API routes
            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

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

            builder.RegisterType<BaseEntity>().As<BaseEntity>().InstancePerRequest();
            builder.RegisterType<EntityService<BaseEntity>>().As<IEntityService<BaseEntity>>().InstancePerRequest();
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
