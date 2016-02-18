using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SampleArch.Startup))]

namespace SampleArch
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            
            //var config = new HttpConfiguration();
            // configure Web API 

            //var log = app.GetLoggerFactory().Create("Site");
            //log.WriteInformation("test");

            //log.IfNotNull(logger => Audit.Log);
            //log.WriteInformation("test2");

            app.MapSignalR();
            
        }
    }
}
