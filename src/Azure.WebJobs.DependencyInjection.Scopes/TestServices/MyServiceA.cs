using System;
using Microsoft.Extensions.Logging;

namespace Azure.WebJobs.DependencyInjection.Scopes.TestServices
{
    public class MyServiceA
    {
        private readonly ILogger<MyServiceA> logger;

        public MyServiceA(ICommonIdProvider idProvider, ILogger<MyServiceA> logger)
        {
            IdProvider = idProvider;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ICommonIdProvider IdProvider { get; }

        public void Dispose()
        {
            logger.LogInformation("Disposing service A {Id}", IdProvider.Id);
        }
    }
}