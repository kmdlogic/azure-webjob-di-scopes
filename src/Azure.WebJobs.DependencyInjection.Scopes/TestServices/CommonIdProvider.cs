using System;
using Microsoft.Extensions.Logging;

namespace Azure.WebJobs.DependencyInjection.Scopes.TestServices
{
    public class CommonIdProvider : IGlobalIdProvider, IDisposable
    {
        private readonly ILogger<CommonIdProvider> logger;

        public CommonIdProvider(ILogger<CommonIdProvider> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string Id { get; } = Guid.NewGuid().ToString();

        public void Dispose()
        {
            logger.LogInformation("Disposing Common ID Provider {Id}", Id);
        }
    }
}