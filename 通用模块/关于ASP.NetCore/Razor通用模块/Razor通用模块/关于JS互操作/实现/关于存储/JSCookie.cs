using Microsoft.AspNetCore;

namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型可以通过JS互操作来索引和修改Cookie
/// </summary>
sealed class JSCookie : JSRuntimeBase, ICookie
{
    #region 公开成员
    #region 关于读取和写入Cookie
    #region 写入原始Cookie
    public ValueTask SetCookieOriginal(string cookie, CancellationToken cancellation = default)
        => JSRuntime.SetProperty("document.cookie", cookie, cancellation);
    #endregion
    #region 读取或写入值（异步索引器）
    public IAsyncIndex<string, string> IndexAsync { get; }
    #endregion
    #region 读取Cookie且不引发异常
    public async Task<(bool Exist, string? Value)> TryGetValueAsync(string key, CancellationToken cancellation = default)
    {
        var value = await JSRuntime.InvokeAsync<string?>("docCookies.getItem", cancellation, key);
        return (value is { }, value);
    }
    #endregion
    #endregion
    #region 关于集合
    #region 枚举所有键值对
    public async IAsyncEnumerator<KeyValuePair<string, string>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        var cookies = await JSRuntime.InvokeAsync<string[]>("docCookies.keys", cancellationToken);
        foreach (var item in cookies)
        {
            yield return new(item, await IndexAsync.Get(item, cancellationToken));
        }
    }
    #endregion
    #region 检查键值对是否存在
    public async Task<bool> ContainsAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => await JSRuntime.InvokeAsync<bool>("docCookies.hasItem", cancellation, item.Key);
    #endregion
    #endregion
    #region 关于添加和删除键值对
    #region 删除指定的键
    public async Task<bool> RemoveAsync(string key, CancellationToken cancellation = default)
        => await JSRuntime.InvokeAsync<bool>("docCookies.removeItem", cancellation, key, "/");
    #endregion
    #region 删除指定的键值对
    public Task<bool> RemoveAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => RemoveAsync(item.Key, cancellation);
    #endregion
    #region 全部删除
    public async Task ClearAsync(CancellationToken cancellation = default)
    {
        foreach (var key in await this.ToListAsync())
        {
            await RemoveAsync(key, cancellation);
        }
    }
    #endregion
    #region 添加键值对
    public Task AddAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => IndexAsync.Set(item.Key, item.Value, cancellation);
    #endregion
    #endregion
    #endregion
    #region 构造函数
    /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
    public JSCookie(IJSRuntime jsRuntime)
        : base(jsRuntime)
    {
        IndexAsync = CreateTasks.AsyncIndex<string, string>(
           async (key, cancellation) => await JSRuntime.InvokeAsync<string>("docCookies.getItem", cancellation, key),
           async (key, value, cancellation) => await JSRuntime.InvokeVoidAsync("docCookies.setItem", cancellation, key, value, 3600 * 24 * 90, "/"));
    }
    #endregion
}
