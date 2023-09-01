namespace Microsoft.AspNetCore.SignalR;

/// <summary>
/// 本类型是一个携带了服务提供者对象的Hub中心
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="serviceProvider">服务提供者对象</param>
public abstract class HubWithService(IServiceProvider serviceProvider) : Hub
{
    #region 服务提供者对象
    /// <summary>
    /// 获取服务提供者对象，
    /// 它可以用于进一步请求其他服务
    /// </summary>
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;

    #endregion
}
