using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.SignalR;
using FluentValidation.WebApi;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SampleArch.WebApi.OwinStartup))]

namespace SampleArch.WebApi
{
    public class OwinStartup
    {
        //public void Configuration(IAppBuilder app)
        //{
        //    // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        //    app.MapSignalR();
        //}
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // STANDARD WEB API SETUP:

            // Get your HttpConfiguration. In OWIN, you'll create one
            // rather than using GlobalConfiguration.
            var config = new HttpConfiguration();

            //// Register your Web API controllers.
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            
            //// Run other optional steps, like registering filters,
            //// per-controller-type services, etc., then set the dependency resolver
            //// to be Autofac.
            ////var container = builder.Build();
            ////config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            ////Autofac Configuration
            ////var builder = new Autofac.ContainerBuilder();
            //Audit.Log.Debug("Application_Start :: Autofac Configuration set");

            //builder.RegisterApiControllers((typeof(WebApiApplication).Assembly)).InstancePerLifetimeScope();
            //Audit.Log.Debug("Application_Start :: RegisterControllers called");

            //builder.RegisterModule(new RepositoryModule());
            //builder.RegisterModule(new ServiceModule());
            //builder.RegisterModule(new EFModule());
            //Audit.Log.Debug("Application_Start :: RegisterModule called");

            // Get your HttpConfiguration.
            //var config = new HttpConfiguration();

            // Register your Web API controllers.
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // You can register hubs all at once using assembly scanning...
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            //builder.RegisterWebApiFilterProvider(config);
            //config.EnsureInitialized();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            //Audit.Log.Debug("Application_Start :: DependencyResolver called");
            

            // OWIN WEB API SETUP:

            // Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            // and finally the standard Web API middleware.
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
            FluentValidationModelValidatorProvider.Configure(config);

            //app.MapSignalR();
        }
    }
}
