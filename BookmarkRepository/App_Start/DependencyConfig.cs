using System.Web.Http.Dependencies;
using BookmarkRepository.Infrastructure;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookmarkRepository
{
    // TODO: this doesn't quite feel right, but it'll do for now.
    public static class DependencyConfig
    {
        private static StandardKernel kernel = new StandardKernel();

        public static void RegisterDependencies()
        {
            kernel.Bind<System.Web.Mvc.IControllerFactory>().To<NinjectControllerFactory>();
            kernel.Bind <IDependencyResolver>().To<NinjectDependencyResolver>();
        }

        public static TInterface Get<TInterface>()
        {
            return kernel.TryGet<TInterface>();
        }
    }
}