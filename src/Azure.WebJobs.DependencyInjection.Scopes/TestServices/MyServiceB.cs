using System;
using Microsoft.Extensions.Logging;

namespace Azure.WebJobs.DependencyInjection.Scopes.TestServices
{
    public class MyServiceB : IDisposable
    {
        private readonly ILogger<MyServiceB> logger;

        public MyServiceB(ICommonIdProvider idProvider, ILogger<MyServiceB> logger)
        {
            IdProvider = idProvider;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ICommonIdProvider IdProvider { get; }

        public void Dispose()
        {
            logger.LogInformation("Disposing service B {Id}", IdProvider.Id);
        }
    }
}