# Azure WebJobs Scoped DI Sample

This sample is based on `IJobActivatorEx` and the example found in the following [comment](https://github.com/Azure/azure-webjobs-sdk/issues/1915#issuecomment-493394658):

```csharp
public class SimpleInjectorJobActivator : IJobActivatorEx
{
    private readonly Container container;

    public SimpleInjectorJobActivator(Container container) => this.container = container;

    public T CreateInstance<T>(IFunctionInstanceEx functionInstance)
    {
        var disposer = functionInstance.InstanceServices.GetRequiredService<ScopeDisposable>();
        this.disposer.Scope = AsyncScopedLifestyle.BeginScope(this.container);
        // Scopes in Simple Injector are ambient; you always resolve from the container
        return (T)this.container.GetInstance(typeof(T));
    }

    // Ensures a created Simple Injector scope is disposed at the end of the request
    public sealed class ScopeDisposable : IDisposable
    {
        public Scope Scope { get; set; }
        public void Dispose() => this.Scope?.Dispose();
    }
}
```

However, here in this example we are using `Microsoft.Extensions.DependencyInjection`.

To run the sample:

1. start your local blob storage emulator (i.e. `docker run -p 10000:10000 mcr.microsoft.com/azure-storage/azurite`)
2. cd to `src/Azure.WebJobs.DependencyInjection.Scopes/`
3. `dotnet run`

You should see output similar to the following:

```
...
[13:18:58 INF] Job host started
Application started. Press Ctrl+C to shut down.
Hosting environment: Development
Content root path: /Users/adamchester/repo/kmd/azure-webjob-di-scopes/src/Azure.WebJobs.DependencyInjection.Scopes/bin/Debug/netcoreapp2.2/
[13:19:03 INF] Executing 'SampleFunction.Run' (Reason='Timer fired at 2019-08-13T13:19:03.4570700+10:00', Id=32ed81a2-7516-482b-a265-7978a554240e)
[13:19:03 INF] Service A ID: 663cbf35-68f7-42a7-8a65-5098d0305b7f. Service B ID: 663cbf35-68f7-42a7-8a65-5098d0305b7f. Global e09db6ff-462b-441e-8567-84958eb21c51. Do A and B have the same Id provider? True
[13:19:03 INF] Executed 'SampleFunction.Run' (Succeeded, Id=32ed81a2-7516-482b-a265-7978a554240e)
[13:19:03 INF] null
[13:19:03 INF] Disposing service B 663cbf35-68f7-42a7-8a65-5098d0305b7f
[13:19:03 INF] Disposing Common ID Provider 663cbf35-68f7-42a7-8a65-5098d0305b7f
[13:19:08 INF] Executing 'SampleFunction.Run' (Reason='Timer fired at 2019-08-13T13:19:08.4458520+10:00', Id=1aaaeaf6-cb0e-49dd-83d8-b3f46aee9c55)
[13:19:08 INF] Service A ID: b88146e1-9a26-4697-9f66-062eea7d72d4. Service B ID: b88146e1-9a26-4697-9f66-062eea7d72d4. Global e09db6ff-462b-441e-8567-84958eb21c51. Do A and B have the same Id provider? True
[13:19:08 INF] Executed 'SampleFunction.Run' (Succeeded, Id=1aaaeaf6-cb0e-49dd-83d8-b3f46aee9c55)
[13:19:08 INF] null
[13:19:08 INF] Disposing service B b88146e1-9a26-4697-9f66-062eea7d72d4
[13:19:08 INF] Disposing Common ID Provider b88146e1-9a26-4697-9f66-062eea7d72d4
[13:19:13 INF] Executing 'SampleFunction.Run' (Reason='Timer fired at 2019-08-13T13:19:13.4520780+10:00', Id=13994fd7-c4c7-45fe-a1f5-8c484fd10e56)
[13:19:13 INF] Service A ID: 08f72e45-7859-4706-ac22-fa7134806e86. Service B ID: 08f72e45-7859-4706-ac22-fa7134806e86. Global e09db6ff-462b-441e-8567-84958eb21c51. Do A and B have the same Id provider? True
[13:19:13 INF] Executed 'SampleFunction.Run' (Succeeded, Id=13994fd7-c4c7-45fe-a1f5-8c484fd10e56)
[13:19:13 INF] null
...
```


Other relevant information:

* https://github.com/Azure/azure-webjobs-sdk/pull/2133
* https://github.com/Azure/azure-webjobs-sdk/issues/2078
* https://github.com/Azure/azure-functions-host/issues/3736
