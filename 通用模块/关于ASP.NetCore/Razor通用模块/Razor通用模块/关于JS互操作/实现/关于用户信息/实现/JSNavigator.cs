
using System.Underlying;

using Microsoft.AspNetCore;

namespace Microsoft.JSInterop;

/// <summary>
/// 本类型是<see cref="IJSNavigator"/>的实现，
/// 可以视为JS中Navigator对象的封装
/// </summary>
/// <param name="jsRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
sealed class JSNavigator(IJSRuntime jsRuntime) : IJSNavigator
{
    #region 获取基本硬件信息
    private IEnvironmentInfoWeb? EnvironmentInfoField;

    public async Task<IEnvironmentInfoWeb> EnvironmentInfo(CancellationToken cancellation = default)
    {
        if (EnvironmentInfoField is { } e)
            return e;
        var userAgent = await jsRuntime.GetProperty<string>("navigator.userAgent", cancellation);
        return EnvironmentInfoField = CreateASP.EnvironmentInfo(userAgent);
    }
    #endregion
    #region 获取定位对象
    public IPosition Geolocation { get; }
        = new JSGeolocation(jsRuntime);
    #endregion
    #region 剪切板对象
    public IJSClipboard Clipboard { get; }
        = new JSClipboard(jsRuntime);
    #endregion
    #region 获取唤醒锁
    public async Task<IAsyncDisposable?> GetWakeLock()
    {
        var id = CreateASP.JSObjectName();
        var success = await jsRuntime.InvokeAsync<bool>("CreateWakeLock", id);
        return success ?
            new JSWakeLock(jsRuntime, id) :
            null;
    }
    #endregion
}
