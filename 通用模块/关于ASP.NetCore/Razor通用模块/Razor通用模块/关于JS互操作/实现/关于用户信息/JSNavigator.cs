
using System.Underlying;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace Microsoft.JSInterop;

/// <summary>
/// 本类型是<see cref="IJSNavigator"/>的实现，
/// 可以视为JS中Navigator对象的封装
/// </summary>
sealed class JSNavigator : JSRuntimeBase, IJSNavigator
{
    #region 获取基本硬件信息
    private IEnvironmentInfoWeb? EnvironmentInfoField;

    public async Task<IEnvironmentInfoWeb> EnvironmentInfo(CancellationToken cancellation = default)
    {
        if (EnvironmentInfoField is { } e)
            return e;
        var userAgent = await JSRuntime.GetProperty<string>("navigator.userAgent", cancellation);
        return EnvironmentInfoField = CreateASP.EnvironmentInfo(userAgent);
    }
    #endregion
    #region 获取或设置剪贴板文本
    private IAsyncProperty<string?>? ClipboardTextFiled;

    public IAsyncProperty<string?> ClipboardText
        => ClipboardTextFiled ??= CreateTasks.AsyncProperty
        (token => JSRuntime.InvokeCodeAsync<string?>("navigator.clipboard.readText()", true, token).AsTask(),
            (value, token) => JSRuntime.InvokeCodeVoidAsync($@"navigator.clipboard.writeText({value.ToJSSecurity()})", true, cancellation: token).AsTask());
    #endregion
    #region 获取定位对象
    public IPosition Geolocation { get; }
    #endregion
    #region 构造函数
    /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
    public JSNavigator(IJSRuntime jsRuntime)
        : base(jsRuntime)
    {
        Geolocation = new JSGeolocation(JSRuntime);
    }
    #endregion
}
