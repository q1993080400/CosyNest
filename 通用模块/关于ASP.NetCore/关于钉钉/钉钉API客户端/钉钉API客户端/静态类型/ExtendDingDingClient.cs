using System.Text.Json;

namespace System;

/// <summary>
/// 有关钉钉客户端的扩展方法全部放在这里
/// </summary>
public static class ExtendDingDingClient
{
    #region 通过JS获取钉钉身份验证结果
    /// <summary>
    /// 通过JS获取钉钉身份验证结果
    /// </summary>
    /// <param name="jsWindow">JS运行时对象</param>
    /// <returns></returns>
    public static async Task<AuthenticationDingDingResult?> GetAuthenticationResult(this IJSWindow jsWindow)
    {
        var key = ToolAuthenticationDingDing.AuthenticationKey;
        var storage = jsWindow.LocalStorage;
        var (_, json) = await storage.TryGetValueAsync(key);
        if (json is null)
            return null;
        try
        {
            return JsonSerializer.Deserialize<AuthenticationDingDingResult>(json);
        }
        catch (JsonException)
        {
            await storage.RemoveAsync(key);
            return null;
        }
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
    public static async Task PersistenceAuthenticationState(this AuthenticationDingDingState? state, IJSWindow jsWindow)
    {
        if (state is { Passed: true, AuthenticationResult: { } result })
        {
            var key = ToolAuthenticationDingDing.AuthenticationKey;
            var json = JsonSerializer.Serialize(result);
            await jsWindow.LocalStorage.IndexAsync.Set(key, json);
            return;
        }
        await jsWindow.LogOutDingDing();
    }
    #endregion
}
