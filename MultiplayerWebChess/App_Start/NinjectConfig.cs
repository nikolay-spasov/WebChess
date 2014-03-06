using MultiplayerWebChess.Domain.DomainContext;
using Ninject;
using System;
using System.Collections.Generic;

namespace MultiplayerWebChess.App_Start
{
    public class NinjectConfig
    {
        private static IKernel kernel;

        public static void Config()
        {
            kernel = new StandardKernel();
            kernel.Bind<IDatabase>().To<DatabaseContext>();
        }

        public static object TryGet(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public static IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}