namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型是剪切板对象在JS上的实现
/// </summary>
/// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
sealed class JSClipboard(IJSRuntime jsRuntime) : JSRuntimeBase(jsRuntime), IJSClipboard
{
    #region 公开成员
    #region 写入剪切板文本
    #region 标准实现
    public Task<bool> Write(string? text, CancellationToken cancellationToken = default)
        => AwaitPromise(_ => true, (successMethod, failMethod) => $"""
        navigator.clipboard.writeText({text.ToJSSecurity()}).
            then({successMethod}).catch({failMethod});
        """, cancellationToken: cancellationToken);
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
    public async Task<(bool IsSuccess, string? Text)> Read(CancellationToken cancellationToken = default)
    {
        var info = await JSRuntime.InvokeAsync<ReadCopyTextInfo>("ReadCopyText");
        return (info.IsSuccess, info.Text);
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
