using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace Website.IoC
{
    public class IoCControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer _container;

        public IoCControllerFactory(IContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) return base.GetControllerInstance(requestContext, null);

            return (IController)_container.GetInstance(controllerType);
        }
    }
}