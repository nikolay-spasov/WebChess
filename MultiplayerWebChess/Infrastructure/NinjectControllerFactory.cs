using System;
using System.Web.Mvc;
using System.Web.Routing;
using MultiplayerWebChess.App_Start;

namespace MultiplayerWebChess.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return NinjectConfig.TryGet(controllerType) as IController;
        }
    }
}