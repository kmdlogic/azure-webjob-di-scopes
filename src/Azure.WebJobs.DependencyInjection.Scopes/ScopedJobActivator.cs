using System;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Azure.WebJobs.DependencyInjection.Scopes
{
    class ScopedJobActivator : IJobActivatorEx
    {
        private readonly IServiceProvider serviceProvider;

        public ScopedJobActivator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public T CreateInstance<T>(IFunctionInstanceEx functionInstance)
        {
            var disposer = functionInstance.InstanceServices.GetRequiredService<ScopeDisposable>();
            disposer.Scope = serviceProvider.CreateScope();
            return serviceProvider.GetRequiredService<T>();
        }

        public T CreateInstance<T>()
        {
            // This will never get called because we're implementing IJobActivatorEx
            throw new NotSupportedException("Cannot create an instance outside of scopes");
        }

        // Ensures a created Simple Injector scope is disposed at the end of the request
        public sealed class ScopeDisposable : IDisposable
        {
            public IServiceScope Scope { get; set; }

            public void Dispose()
            {
                Log.Information("The scope is disposing");
                Scope?.Dispose();
            }
        }
    }
}