using System.SafetyFrancis.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个静态类可以用来创建和Razor有关的对象
/// </summary>
public static class CreateRazor
{
    #region 创建Cookie身份验证对象
    /// <summary>
    /// 创建一个<see cref="IAuthentication{Parameter}"/>对象，
    /// 它通过在Cookie中写入和移除键值对来执行登录和注销操作
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="AuthenticationCookie.AuthenticationCookie(IJSWindow, string)"/>
    public static IAuthentication<string> AuthenticationCookie(IJSWindow jsWindow, string key)
        => new AuthenticationCookie(jsWindow, key);
    #endregion
}
