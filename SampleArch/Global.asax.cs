using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using Autofac;
using FluentValidation.Mvc;
using SampleArch.Logging;
using SampleArch.Modules;

namespace SampleArch
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            Audit.Log.Debug("Application_Start");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FluentValidationModelValidatorProvider.Configure();

            //Autofac Configuration
            var builder = new Autofac.ContainerBuilder();
            Audit.Log.Debug("Application_Start :: Autofac Configuration set");

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            Audit.Log.Debug("Application_Start :: RegisterControllers called");

            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EfModule());
            Audit.Log.Debug("Application_Start :: RegisterModule called");

            var container = builder.Build();
            
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            Audit.Log.Debug("Application_Start :: DependencyResolver called");
        }
    }
}
