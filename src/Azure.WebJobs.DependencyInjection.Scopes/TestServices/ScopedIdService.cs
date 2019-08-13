using System;

namespace Azure.WebJobs.DependencyInjection.Scopes.TestServices
{
    public class ScopedIdService : IScopedIdService
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}