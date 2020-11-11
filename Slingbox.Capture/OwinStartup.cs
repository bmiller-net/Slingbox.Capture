using System.Configuration;
using System.Net;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Components;
using Ninject.Web.Common;
using Owin;
using Slingbox.API;
using Slingbox.API.Model;
using WebApiContrib.IoC.Ninject;

namespace Slingbox.Capture
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var ipAddress = ConfigurationManager.AppSettings[AppSettingsEnum.Slingbox_IPAddress.ToString()].ToString();
            var port = int.Parse(ConfigurationManager.AppSettings[AppSettingsEnum.Slingbox_Port.ToString()].ToString());
            var username = ConfigurationManager.AppSettings[AppSettingsEnum.Slingbox_Username.ToString()].ToString();
            var password = ConfigurationManager.AppSettings[AppSettingsEnum.Slingbox_AdminPassword.ToString()].ToString();

            var kernel = new StandardKernel();

            kernel.Bind<VideoStream>().To<VideoStream>().InSingletonScope();
            kernel.Bind<SlingboxService>().To<SlingboxService>()
                .InSingletonScope()
                .WithConstructorArgument("ipAddress", ipAddress)
                .WithConstructorArgument("port", port)
                .WithConstructorArgument("username", username)
                .WithConstructorArgument("password", password);
            
            var resolver = new NinjectResolver(kernel);

            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
            );

            config.DependencyResolver = resolver;

            appBuilder.UseWebApi(config);
        }
    }
}
