using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StructureMap;
using Website.IoC;

namespace Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            IContainer container = DependencyResolution.IoC.Initialize();
            AutoMapperConfiguration.Configure(container);
            ControllerConfiguration.Configure(container);
            FluentValidationConfiguration.Configure();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
