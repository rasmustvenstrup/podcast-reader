using System.Web.Mvc;
using StructureMap;

namespace Website.IoC
{
    public static class ControllerConfiguration
    {
        public static void Configure(IContainer container)
        {
            ControllerBuilder.Current.SetControllerFactory(new IoCControllerFactory(container));
        }
    }
}