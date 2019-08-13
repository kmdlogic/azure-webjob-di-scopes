using System;

namespace Azure.WebJobs.DependencyInjection.Scopes.TestServices
{
    public interface IScopedIdService
    {
        Guid Id { get; }
    }
}
