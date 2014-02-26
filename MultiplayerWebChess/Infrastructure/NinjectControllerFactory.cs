using MultiplayerWebChess.Domain.DomainContext;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
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