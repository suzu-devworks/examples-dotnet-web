namespace Examples.Web.Generator.JwtToken.Services;

public class SimpleServiceProvider : IServiceProvider, IDisposable, IAsyncDisposable
{
    private readonly Dictionary<Type, object> _services = new();
    private bool _disposed;

    public void Register<T>(T implementation) where T : class
        => _services[typeof(T)] = implementation;

    public object? GetService(Type serviceType)
        => _services.TryGetValue(serviceType, out var service) ? service : null;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) { return; }
        foreach (var service in _services.Values)
        {
            if (service is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
            else if (service is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) { return; }

        foreach (var service in _services.Values.OfType<IDisposable>())
        {
            service.Dispose();
        }

        _disposed = true;
    }
}
