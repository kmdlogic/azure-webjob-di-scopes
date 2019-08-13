using System;
using System.Threading.Tasks;
using Azure.WebJobs.DependencyInjection.Scopes.TestServices;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Azure.WebJobs.DependencyInjection.Scopes
{
    internal static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console().CreateLogger();

            try
            {
                Log.Information("Starting");

                var hostBuilder = new HostBuilder()
                    .UseEnvironment("Development")
                    .ConfigureAppConfiguration(builder => { builder.AddJsonFile("appsettings.json", optional: false); })
                    .ConfigureWebJobs(c => { c.AddTimers().AddAzureStorageCoreServices().AddAzureStorage(); })
                    .ConfigureLogging(l => { l.ClearProviders().AddSerilog(); })
                    .UseConsoleLifetime()
                    .ConfigureServices((context, services) =>
                    {
                        services
                            .AddScoped<IScopedIdService, ScopedIdService>()
                            .AddScoped<ScopedJobActivator.ScopeDisposable>();

                        // Register MyServiceA as transient.
                        // A new instance will be returned every
                        // time a service request is made
                        services.AddTransient<MyServiceA>();

                        // Register MyServiceB as scoped.
                        // The same instance will be returned
                        // within the scope of a function invocation
                        services.AddScoped<MyServiceB>();

                        // Register ICommonIdProvider as scoped.
                        // The same instance will be returned
                        // within the scope of a function invocation
                        services.AddScoped<ICommonIdProvider, CommonIdProvider>();

                        // Register IGlobalIdProvider as singleton.
                        // A single instance will be created and reused
                        // with every service request
                        services.AddSingleton<IGlobalIdProvider, CommonIdProvider>();

                        // This must be done *after* everything is added to the container
                        services.AddSingleton<IJobActivatorEx>(new ScopedJobActivator(services.BuildServiceProvider()));
                    });

                using (var host = hostBuilder.Build())
                {
                    await host.RunAsync();
                }

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fatal exception");
                return 1;
            }
            finally
            {
                Log.Information("Shutting down");
                Log.CloseAndFlush();
            }
        }
    }
}