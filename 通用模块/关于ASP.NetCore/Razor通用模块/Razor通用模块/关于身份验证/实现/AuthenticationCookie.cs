using System.SafetyFrancis.Authentication;

using Microsoft.JSInterop;

namespace Microsoft.AspNetCore.Authorization;

/// <summary>
/// 这个类型是<see cref="IAuthentication{Parameter}"/>的实现，
/// 它可以通过向Cookie写入和移除数据来实现验证和注销
/// </summary>
sealed class AuthenticationCookie : IAuthentication<string>
{
    #region 公开成员
    #region 登录
    public async Task Sign(string parameter, CancellationToken cancellation)
    {
        await JSWindow.Document.Cookie.IndexAsync.Set(Key, parameter, cancellation);
        await JSWindow.Location.Reload();
    }
    #endregion
    #region 注销
    public async Task LogOut(CancellationToken cancellation)
    {
        await JSWindow.Document.Cookie.RemoveAsync(Key, cancellation);
        await JSWindow.Location.Reload();
    }
    #endregion 
    #endregion
    #region 内部成员
    #region JS运行时
    /// <summary>
    /// 获取封装的JS运行时对象，
    /// 它被用来写入或移除Cookie
    /// </summary>
    private IJSWindow JSWindow { get; }
    #endregion
    #region Cookie的键
    /// <summary>
    /// 获取用来写入或移除Cookie的键
    /// </summary>
    private string Key { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="jsWindow">封装的JS运行时对象，
    /// 它被用来写入或移除Cookie</param>
    /// <param name="key">用来写入或移除Cookie的键</param>
    public AuthenticationCookie(IJSWindow jsWindow, string key)
    {
        JSWindow = jsWindow;
        Key = key;
    }
    #endregion
}
