using Microsoft.AspNet.SignalR;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultiplayerWebChess.App_Start;

namespace MultiplayerWebChess.Infrastructure
{
    public class SignalRResolver : DefaultDependencyResolver
    {
        public override object GetService(Type serviceType)
        {
            return NinjectConfig.TryGet(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return NinjectConfig.GetServices(serviceType).Concat(base.GetServices(serviceType));
        }
    }
}