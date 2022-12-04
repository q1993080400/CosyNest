namespace System.SafetyFrancis.Authentication;

/// <summary>
/// 凡是实现这个接口的类型，都可以进行登录和注销操作，
/// 请注意：登录和注销有可能在前端进行，后端不一定会承认
/// </summary>
/// <typeparam name="Parameter">登陆参数的类型</typeparam>
public interface IAuthentication<in Parameter>
{
    #region 登录
    /// <summary>
    /// 执行登录
    /// </summary>
    /// <param name="parameter">用于登录的参数</param>
    /// <param name="cancellation">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task Sign(Parameter parameter, CancellationToken cancellation = default);
    #endregion
    #region 注销
    /// <summary>
    /// 执行注销操作
    /// </summary>
    /// <param name="cancellation">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task LogOut(CancellationToken cancellation = default);
    #endregion
}
