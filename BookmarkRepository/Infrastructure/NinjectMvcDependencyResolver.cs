﻿using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookmarkRepository.Infrastructure
{
    public class NinjectMvcDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectMvcDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }

}