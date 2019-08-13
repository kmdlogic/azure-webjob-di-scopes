// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading.Tasks;
using Azure.WebJobs.DependencyInjection.Scopes.TestServices;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Serilog;

namespace Azure.WebJobs.DependencyInjection.Scopes
{
    // ReSharper disable once UnusedMember.Global
    public class SampleFunction
    {
        private readonly MyServiceA _serviceA;
        private readonly MyServiceB _serviceB;
        private readonly IGlobalIdProvider _globalIdProvider;

        public SampleFunction(MyServiceA serviceA, MyServiceB serviceB, IGlobalIdProvider globalIdProvider)
        {
            _serviceA = serviceA ?? throw new ArgumentNullException(nameof(serviceA));
            _serviceB = serviceB ?? throw new ArgumentNullException(nameof(serviceB));
            _globalIdProvider = globalIdProvider ?? throw new ArgumentNullException(nameof(globalIdProvider));
        }

        public Task Run(
            [TimerTrigger("00:00:05")] TimerInfo timer)
        {
            var isSameIdProviderInstance = ReferenceEquals(_serviceA.IdProvider, _serviceB.IdProvider);

            Log.Information(
                "Service A ID: {ServiceAId}. Service B ID: {ServiceBId}. Global {GlobalId}. Do A and B have the same Id provider? {IsSameIdProviderInstance}",
                _serviceA.IdProvider.Id,
                _serviceB.IdProvider.Id,
                _globalIdProvider.Id,
                isSameIdProviderInstance);

            return Task.CompletedTask;
        }
    }
}