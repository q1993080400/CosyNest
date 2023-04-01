namespace Microsoft.AspNetCore.SignalR;

/// <summary>
/// 本类型是一个携带了服务提供者对象的Hub中心
/// </summary>
public abstract class HubWithService : Hub
{
    #region 服务提供者对象
    /// <summary>
    /// 获取服务提供者对象，
    /// 它可以用于进一步请求其他服务
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="serviceProvider">服务提供者对象</param>
    public HubWithService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
    #endregion
}
