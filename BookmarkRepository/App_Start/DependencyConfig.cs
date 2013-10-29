using System.Web.Http.Dependencies;
using BookmarkRepository.Infrastructure;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Raven.Client.Embedded;

namespace BookmarkRepository
{
    // TODO: this doesn't quite feel right, but it'll do for now.
    public static class DependencyConfig
    {
        private static StandardKernel kernel = new StandardKernel();

        public static System.Web.Mvc.IDependencyResolver MvcDependencyResolver { get { return kernel.Get<System.Web.Mvc.IDependencyResolver>(); } }
        public static System.Web.Http.Dependencies.IDependencyResolver HttpDependencyResolver { get { return kernel.Get<System.Web.Http.Dependencies.IDependencyResolver>(); } }

        public static void RegisterDependencies()
        {
            kernel.Bind<IDocumentStore>().ToConstant(new EmbeddableDocumentStore { ConnectionStringName = "RavenDb" }.Initialize());
            kernel.Bind<IDocumentSession>().ToMethod(c => c.Kernel.Get<IDocumentStore>().OpenSession());
            kernel.Bind<System.Web.Mvc.IDependencyResolver>().To<NinjectMvcDependencyResolver>().InSingletonScope();
            kernel.Bind<System.Web.Http.Dependencies.IDependencyResolver>().To<NinjectHttpDependencyResolver>();
        }

        public static TInterface Get<TInterface>()
        {
            return kernel.TryGet<TInterface>();
        }
    }
}