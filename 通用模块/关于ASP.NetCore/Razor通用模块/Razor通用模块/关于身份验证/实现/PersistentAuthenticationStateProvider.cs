using System.SafetyFrancis;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="AuthenticationStateProvider"/>的实现，
/// 它可以从预呈现状态中获取身份验证信息，并持久化到客户端中，
/// 除非刷新客户端，否则身份验证信息不会改变
/// </summary>
/// <param name="persistentComponentState">这个对象可以从预呈现状态中提取身份验证信息</param>
/// <param name="getUser">这个委托的参数是管理预呈现状态的对象，
/// 返回值是储存在预呈现状态中的身份验证信息，
/// 如果为<see langword="null"/>，表示没有身份验证信息</param>
sealed class PersistentAuthenticationStateProvider
    (PersistentComponentState persistentComponentState,
    Func<PersistentComponentState, ClaimsPrincipal?> getUser) : AuthenticationStateProvider
{
    #region 抽象类实现
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var state = new AuthenticationState(User);
        return Task.FromResult(state);
    }
    #endregion
    #region 内部成员
    #region 经过身份验证的用户对象
    /// <summary>
    /// 获取经过身份验证的用户对象，
    /// 它在客户端的生命周期始终保持不变
    /// </summary>
    private ClaimsPrincipal User { get; } = getUser(persistentComponentState) ?? CreateSafety.PrincipalDefault();
    #endregion
    #endregion
}
