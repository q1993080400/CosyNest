namespace Microsoft.AspNetCore;

/// <summary>
/// 这个静态类可以用来创建和钉钉中台有关的对象
/// </summary>
public static class CreateDingDingShared
{
    #region 创建钉钉身份验证信息获取器
    /// <summary>
    /// 创建一个<see cref="IGetAuthenticationDingDingState"/>，
    /// 它可以用来获取钉钉身份验证信息
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetAuthenticationDingDingStateDefault(DingDingWebApiInfo)"/>
    public static IGetAuthenticationDingDingState GetAuthenticationDingDingState(DingDingWebApiInfo info)
        => new GetAuthenticationDingDingStateDefault(info);
    #endregion
}
