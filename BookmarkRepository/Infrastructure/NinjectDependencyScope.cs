using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Syntax;

namespace BookmarkRepository.Infrastructure
{
    public class NinjectDependencyScope : IDependencyScope
    {
        private IResolutionRoot resolver;

        public NinjectDependencyScope(IResolutionRoot resolver)
        {
            this.resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            ThrowIfDisposed();
            return resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            ThrowIfDisposed();
            return resolver.GetAll(serviceType);
        }

        private void ThrowIfDisposed()
        {
            if (resolver == null)
                throw new ObjectDisposedException(GetType().Name);
        }

        public void Dispose()
        {
            var disposable = resolver as IDisposable;
            if (disposable != null)
                disposable.Dispose();

            resolver = null;
        }
    }
}