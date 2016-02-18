using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SampleArch.Logging;

namespace SampleArch.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Audit.Log.Debug("Application_Start :: App Starting ...");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Audit.Log.Debug("Application_Start :: Registered Configs");

            ////Autofac Configuration
            //var builder = new Autofac.ContainerBuilder();
            //Audit.Log.Debug("Application_Start :: Autofac Configuration set");

            //builder.RegisterModule(new RepositoryModule());
            //builder.RegisterModule(new ServiceModule());
            //builder.RegisterModule(new EFModule());
            //Audit.Log.Debug("Application_Start :: RegisterModule called");

            ////builder.RegisterApiControllers((typeof(WebApiApplication).Assembly)).PropertiesAutowired().InstancePerLifetimeScope();
            //Audit.Log.Debug("Application_Start :: RegisterControllers called");
            
            //// Get your HttpConfiguration.
            //var config = new HttpConfiguration();

            //// Register your Web API controllers.
            ////builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //// You can register hubs all at once using assembly scanning...
            ////builder.RegisterHubs(Assembly.GetExecutingAssembly());

            //// OPTIONAL: Register the Autofac filter provider.
            ////builder.RegisterWebApiFilterProvider(config);

            //// Set the dependency resolver to be Autofac.
            //var container = builder.Build();
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            //Audit.Log.Debug("Application_Start :: DependencyResolver called");

        }
    }
}
