namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型是剪切板对象在JS上的实现
/// </summary>
/// <param name="jsRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
sealed class JSClipboard(IJSRuntime jsRuntime) : IJSClipboard
{
    #region 公开成员
    #region 写入剪切板文本
    #region 标准实现
    public async Task<bool> WriteText(string? text, CancellationToken cancellationToken = default)
        => await jsRuntime.InvokeAsync<bool>("WriteClipboardText", cancellationToken, text);
    #endregion
    #region 为IOS优化后的实现
    public string WriteTextScript(string getCopyTextScript, string callbackFunctionName)
        => $"""
        navigator.clipboard.writeText({getCopyTextScript}).
            then(x=>{callbackFunctionName}(true)).
            catch(x=>{callbackFunctionName}(false));
        """;
    #endregion
    #endregion
    #region 读取剪切板文本
    public async Task<(bool IsSuccess, string? Text)> ReadText(CancellationToken cancellationToken = default)
    {
        var info = await jsRuntime.InvokeAsync<ReadCopyTextInfo>("ReadClipboardText", cancellationToken);
        return (info.IsSuccess, info.Text);
    }
    #endregion
    #region 读取剪切板的内容
    public async Task<IClipboardItems?> ReadObject(bool withObjectURL = false)
    {
        var items = await jsRuntime.InvokeAsync<ClipboardItem[]?>("ReadClipboardObject", withObjectURL);
        return items is null ? null : new ClipboardItemsRealize(items);
    }
    #endregion
    #endregion
    #region 内部类型
    /// <summary>
    /// 这个内部类型被用来封装JS互操作返回值
    /// </summary>
    private sealed record ReadCopyTextInfo
    {
        public bool IsSuccess { get; set; }
        public string? Text { get; set; }
    }
    #endregion
}
