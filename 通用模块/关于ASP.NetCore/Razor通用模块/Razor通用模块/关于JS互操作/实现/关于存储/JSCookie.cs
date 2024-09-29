using Microsoft.AspNetCore;

namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型可以通过JS互操作来索引和修改Cookie
/// </summary>
sealed class JSCookie : ICookie
{
    #region 公开成员
    #region 关于读取和写入Cookie
    #region 写入原始Cookie
    public async ValueTask SetCookieOriginal(string cookie, CancellationToken cancellation = default)
        => await JSRuntime.SetProperty("document.cookie", cookie, cancellation);
    #endregion
    #region 读取或写入值（异步索引器）
    public IAsyncIndex<string, string> IndexAsync { get; }
    #endregion
    #region 读取Cookie且不引发异常
    public async Task<(bool Exist, string? Value)> TryGetValueAsync(string key, CancellationToken cancellation = default)
    {
        var result = await JSRuntime.InvokeAsync<ExistAndValue>("docCookies.tryGetValue", cancellation, key);
        return (result.Exist, result.Value);
    }
    #endregion
    #endregion
    #region 关于集合
    #region 枚举所有键值对
    public async IAsyncEnumerator<KeyValuePair<string, string>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        var cookie = await JSRuntime.InvokeAsync<KeyAndValue[]>("docCookies.keyAndValue", cancellationToken);
        foreach (var item in cookie)
        {
            yield return new(item.Key, item.Value);
        }
    }
    #endregion
    #region 检查键值对是否存在
    public async Task<bool> ContainsAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => await JSRuntime.InvokeAsync<bool>("docCookies.hasItem", cancellation, item.Key);
    #endregion
    #region 返回元素数量
    public async Task<int> CountAsync(CancellationToken cancellation = default)
        => await JSRuntime.InvokeAsync<int>("docCookies.count", cancellation, null);
    #endregion
    #endregion
    #region 关于添加和删除键值对
    #region 删除指定的键
    public async Task<bool> RemoveAsync(string key, CancellationToken cancellation = default)
        => await JSRuntime.InvokeAsync<bool>("docCookies.removeItem", cancellation, key, "/");
    #endregion
    #region 删除指定的键值对
    public async Task<bool> RemoveAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => await RemoveAsync(item.Key, cancellation);
    #endregion
    #region 全部删除
    public async Task ClearAsync(CancellationToken cancellation = default)
        => await JSRuntime.InvokeVoidAsync("docCookies.clear", cancellation, "/");
    #endregion
    #region 添加键值对
    public async Task AddAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => await IndexAsync.Set(item.Key, item.Value, cancellation);
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 指定的JS运行时对象
    /// <summary>
    /// 获取指定的JS运行时对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private IJSRuntime JSRuntime { get; }
    #endregion
    #region 辅助类型
    private sealed record ExistAndValue
    {
        public bool Exist { get; set; }
        public string? Value { get; set; }
    }

    private sealed record KeyAndValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的JS运行时初始化对象
    /// </summary>
    /// <param name="jsRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
    public JSCookie(IJSRuntime jsRuntime)
    {
        JSRuntime = jsRuntime;
        IndexAsync = CreateTasks.AsyncIndex<string, string>(
           async (key, cancellation) =>
           {
               var (_, value) = await TryGetValueAsync(key, cancellation);
               return value ?? "";
           },
           async (key, value, cancellation) =>
           {
               await JSRuntime.InvokeVoidAsync("docCookies.setItem", cancellation, key, value, 3600 * 24 * 90, "/");
           });
    }
    #endregion
}
