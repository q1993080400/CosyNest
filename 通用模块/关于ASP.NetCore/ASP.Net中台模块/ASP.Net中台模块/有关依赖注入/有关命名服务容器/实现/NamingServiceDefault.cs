namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 该类型是<see cref="INamingServiceUnite{Service}"/>的默认实现
/// </summary>
/// <inheritdoc cref="INamingService{Service}"/>
sealed class NamingServiceDefault<Service> : INamingServiceUnite<Service>
    where Service : class
{
    #region 封装的对象
    #region 提供服务的委托
    /// <summary>
    /// 这个委托的第一个参数是服务的名称，
    /// 第二个参数是用来请求服务依赖的服务的对象，
    /// 返回值就是请求到的服务，如果没有服务，则返回<see langword="null"/>
    /// </summary>
    private Func<string, IServiceProvider, Service?> ServiceProvider { get; }
    #endregion
    #region 缓存服务的字典
    /// <summary>
    /// 这个字典可以通过名称来检索已被缓存的服务
    /// </summary>
    private IDictionary<string, Service> Services { get; } = new Dictionary<string, Service>();
    #endregion
    #region 服务请求对象
    /// <summary>
    /// 获取一个对象，
    /// 它可以用来请求服务所依赖的服务
    /// </summary>
    private IServiceProvider DependProvider { get; }
    #endregion
    #region 基础服务容器
    /// <summary>
    /// 如果该对象不为<see langword="null"/>，
    /// 则本对象先向它请求服务，如果请求不到再在本对象中请求服务
    /// </summary>
    private INamingService<Service>? Base { get; }
    #endregion
    #endregion
    #region 同步获取服务
    public Service this[string serviceName, bool @throw = true]
        => Services.TrySetValue(serviceName,
                 name => ServiceProvider(name, DependProvider) ??
                 (@throw ? throw new KeyNotFoundException($"不存在名为{name}的服务") : null!)).Value;
    #endregion
    #region 释放对象
    public void Dispose()
    {
        if (typeof(IDisposable).IsAssignableFrom(typeof(Service)))
        {
            foreach (var item in Services.Values.Cast<IDisposable>())
            {
                item.Dispose();
            }
        }
        Base?.Dispose();
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 通过指定的参数创建对象
    /// </summary>
    /// <param name="dependProvider">该对象可以用来请求服务所依赖的服务</param>
    /// <param name="serviceProvider">这个委托的第一个参数是服务的名称，
    /// 第二个参数是用来请求服务依赖的服务的对象，返回值就是请求到的服务，
    /// 如果没有服务，则返回<see langword="null"/></param>
    /// <param name="base">如果该对象不为<see langword="null"/>，
    /// 则本对象先向它请求服务，如果请求不到再在本对象中请求服务</param>
    public NamingServiceDefault(IServiceProvider dependProvider, Func<string, IServiceProvider, Service?> serviceProvider, INamingService<Service>? @base)
    {
        this.DependProvider = dependProvider;
        this.Base = @base;
        this.ServiceProvider = Base is null ? serviceProvider :
            (name, provider) => Base[name, false] ?? serviceProvider(name, provider);
    }
    #endregion
}
