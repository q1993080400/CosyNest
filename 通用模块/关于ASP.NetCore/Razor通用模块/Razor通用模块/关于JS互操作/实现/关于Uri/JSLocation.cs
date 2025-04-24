namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型是<see cref="IJSLocation"/>的实现，
/// 可以视为一个JS中的Location对象
/// </summary>
/// <param name="jsRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
sealed class JSLocation(IJSRuntime jsRuntime) : IJSLocation
{
    #region 关于Uri
    #region 获取或设置当前Uri
    public IAsyncProperty<string> Href { get; }
        = jsRuntime.PackProperty<string>("location.href");
    #endregion
    #region 获取主机名称
    public ValueTask<string> Host(CancellationToken cancellation)
        => jsRuntime.GetProperty<string>("location.host", cancellation);
    #endregion
    #region 获取协议部分
    public ValueTask<string> Protocol(CancellationToken cancellation)
        => jsRuntime.GetProperty<string>("location.protocol", cancellation);
    #endregion
    #region 获取源部分
    public ValueTask<string> Origin(CancellationToken cancellation = default)
        => jsRuntime.GetProperty<string>("location.origin", cancellation);
    #endregion
    #region 获取或设置锚部分
    public IAsyncProperty<string> Hash { get; }
        = jsRuntime.PackProperty<string>("location.hash");
    #endregion
    #endregion
    #region 刷新页面
    public ValueTask Reload(CancellationToken cancellation = default)
        => jsRuntime.InvokeVoidAsync("location.reload", cancellation);
    #endregion
}
