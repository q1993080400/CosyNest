namespace System.Design;

/// <summary>
/// 这个类型允许合并多个<see cref="IServiceProvider"/>，
/// 依次通过它们请求服务，直到请求成功为止
/// </summary>
sealed class ServiceProviderMerge(IServiceProvider[] serviceProviders) : IServiceProvider
{
    #region 请求服务
    public object? GetService(Type serviceType)
    {
        foreach (var serviceProvider in serviceProviders)
        {
            var service = serviceProvider.GetService(serviceType);
            if (service is { })
                return service;
        }
        return null;
    }
    #endregion 
}
