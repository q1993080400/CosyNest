﻿namespace System.DingDing;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都具有一个钉钉身份验证状态，按照规范，
/// 所有有关钉钉的WebApi的返回值，都应该实现这个接口
/// </summary>
public interface IHasAuthenticationDingDingState
{
    #region 身份验证状态
    /// <summary>
    /// 获取当前身份验证状态
    /// </summary>
    AuthenticationDingDingState AuthenticationState { get; }
    #endregion
}
