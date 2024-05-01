using System.NetFrancis.Http;

namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来获取钉钉登录的状态
/// </summary>
public interface IGetAuthenticationDingDingState
{
    #region 获取身份验证状态
    /// <summary>
    /// 获取钉钉身份验证状态
    /// </summary>
    /// <param name="parameter">用来获取身份验证状态的参数</param>
    /// <returns></returns>
    [HttpMethodPost]
    Task<APIPackDingDing> GetAuthenticationDingDingState
        (AuthenticationDingDingRequest parameter);
    #endregion
    #region 获取App信息
    /// <summary>
    /// 获取钉钉应用的App信息
    /// </summary>
    /// <returns></returns>
    Task<DingDingAppInfo> GetAppInfo();
    #endregion
}
