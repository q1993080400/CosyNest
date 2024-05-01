using System.Text.Json;

namespace Microsoft.AspNetCore;

/// <summary>
/// 有关钉钉客户端的扩展方法全部放在这里
/// </summary>
public static class ExtendDingDingClient
{
    #region 通过JS获取钉钉身份验证请求
    /// <summary>
    /// 通过JS获取钉钉身份验证请求
    /// </summary>
    /// <param name="jsWindow">JS运行时对象</param>
    /// <returns></returns>
    public static async Task<AuthenticationDingDingRequest?> GetAuthenticationRequest(this IJSWindow jsWindow)
    {
        var (_, json) = await jsWindow.LocalStorage.TryGetValueAsync(ToolAuthenticationDingDing.AuthenticationKey);
        if (json is null)
            return null;
        return JsonSerializer.Deserialize<AuthenticationDingDingRequest>(json);
    }
    #endregion
    #region 注销钉钉身份验证
    /// <summary>
    /// 注销钉钉身份验证
    /// </summary>
    /// <param name="jsWindow">JS运行时对象</param>
    /// <returns></returns>
    public static async Task LogOutDingDing(this IJSWindow jsWindow)
    {
        await jsWindow.LocalStorage.RemoveAsync(ToolAuthenticationDingDing.AuthenticationKey);
        var href = jsWindow.Location.Href;
        var uri = await href.Get();
        var host = new UriComplete(uri).UriHost!;
        await href.Set(host);
    }
    #endregion
    #region 持久化身份验证状态
    /// <summary>
    /// 将身份验证状态持久化到客户端
    /// </summary>
    /// <param name="state">要持久化的身份验证状态，
    /// 如果为<see langword="null"/>，会退出登录</param>
    /// <param name="jsWindow">JS运行时对象</param>
    /// <returns></returns>
    public static async Task PersistenceAuthorizationState(this AuthorizationDingDingState? state, IJSWindow jsWindow)
    {
        switch (state)
        {
            case null:
                await jsWindow.LogOutDingDing();
                break;
            case { AuthenticationState: { Passed: true, NextRequest: { } nextRequest } }:
                var key = ToolAuthenticationDingDing.AuthenticationKey;
                var json = JsonSerializer.Serialize(nextRequest);
                await jsWindow.LocalStorage.IndexAsync.Set(key, json);
                break;
        }
    }
    #endregion
}
