namespace WebUI.Infrastructure
{
    using Domain;
    using Domain.Concrete;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using WebUI.Infrastructure.Abstract;
    using WebUI.Infrastructure.Concrete;

    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IRepository>().To<EFRepository>();
            kernel.Bind<IAuthProvider>().To<FormAuthProvider>();
        }
    }
}