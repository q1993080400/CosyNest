using System.Security.Claims;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个对象是<see cref="AuthenticationStateProvider"/>的实现，
/// 它通过服务端进行身份验证，并通过预呈现状态将身份验证结果传给客户端，
/// 每隔一段时间，都会重新刷新验证
/// </summary>
sealed class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
    #region 抽象类实现
    #region 获取验证状态是否有效
    protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        => Task.FromResult(true);
    #endregion
    #region 重新验证时间
    protected override TimeSpan RevalidationInterval { get; }
    #endregion
    #region 释放对象
    protected override void Dispose(bool disposing)
    {
        Subscription.Dispose();
        base.Dispose(disposing);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 用来注销事件的对象
    /// <summary>
    /// 通过释放这个对象，
    /// 可以用来注销预呈现期间绑定的事件
    /// </summary>
    private IDisposable Subscription { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="persistentComponentState">用来在预呈现阶段写入状态的对象</param>
    /// <param name="setState">这个委托的第一个参数是当前用户，
    /// 第二个参数是在预呈现阶段写入状态的对象</param>
    /// <param name="loggerFactory">用来写入日志的对象</param>
    /// <param name="revalidationInterval">指定刷新登陆状态的时间，
    /// 如果为<see langword="null"/>，默认为30分钟</param>
    public PersistingRevalidatingAuthenticationStateProvider
        (PersistentComponentState persistentComponentState,
        Func<ClaimsPrincipal, PersistentComponentState, Task> setState,
        ILoggerFactory loggerFactory,
        TimeSpan? revalidationInterval)
        : base(loggerFactory)
    {
        RevalidationInterval = revalidationInterval ?? TimeSpan.FromMinutes(30);
        Subscription = persistentComponentState.RegisterOnPersisting(async () =>
        {
            var user = (await GetAuthenticationStateAsync()).User;
            await setState(user, persistentComponentState);
        }, RenderMode.InteractiveWebAssembly);
    }
    #endregion
}
