namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型是<see cref="IJSLocation"/>的实现，
/// 可以视为一个JS中的Location对象
/// </summary>
sealed class JSLocation : JSRuntimeBase, IJSLocation
{
    #region 关于Uri
    #region 获取或设置当前Uri
    private IAsyncProperty<string>? HrefField;

    public IAsyncProperty<string> Href
        => HrefField ??= JSRuntime.PackProperty<string>("location.href");
    #endregion
    #region 获取主机名称
    public ValueTask<string> Host(CancellationToken cancellation)
        => JSRuntime.GetProperty<string>("location.host", cancellation);
    #endregion
    #region 获取协议部分
    public ValueTask<string> Protocol(CancellationToken cancellation)
          => JSRuntime.GetProperty<string>("location.protocol", cancellation);
    #endregion
    #region 获取或设置锚部分
    private IAsyncProperty<string>? HashField;

    public IAsyncProperty<string> Hash
        => HashField ??= JSRuntime.PackProperty<string>("location.hash");
    #endregion
    #endregion
    #region 刷新页面
    public ValueTask Reload(bool forceGet = false, CancellationToken cancellation = default)
        => JSRuntime.InvokeVoidAsync("location.reload", cancellation, forceGet);
    #endregion
    #region 构造函数
    /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
    public JSLocation(IJSRuntime jsRuntime)
        : base(jsRuntime)
    {

    }
    #endregion
}
