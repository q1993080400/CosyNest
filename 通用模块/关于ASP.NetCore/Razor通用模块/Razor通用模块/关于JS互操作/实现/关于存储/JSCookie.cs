using System.Text.RegularExpressions;

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
        var kv = await this.Linq(x => x.FirstOrDefault(y => y.Key == key));
        return kv.Equals(default(KeyValuePair<string, string>)) ? (false, null) : (true, kv.Value);
    }
    #endregion
    #endregion
    #region 关于集合
    #region 枚举所有键值对
    public async IAsyncEnumerator<KeyValuePair<string, string>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        foreach (var item in ToolRegex.KeyValuePairExtraction(await GetCookie(cancellationToken), ";"))
        {
            yield return item;
        }
    }
    #endregion
    #region 检查键值对是否存在
    public async Task<bool> ContainsAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => (await TryGetValueAsync(item.Key, cancellation)) is (true, var value) && Equals(value, item.Value);
    #endregion
    #endregion
    #region 关于添加和删除键值对
    #region 删除指定的键
    public async Task<bool> RemoveAsync(string key, CancellationToken cancellation = default)
    {
        if (await this.To<IAsyncDictionary<string, string>>().ContainsKeyAsync(key, cancellation))
        {
            await SetCookie($"{key}=; expires={MinDate}", cancellation);
            return true;
        }
        return false;
    }
    #endregion
    #region 删除指定的键值对
    public Task<bool> RemoveAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => RemoveAsync(item.Key, cancellation);
    #endregion
    #region 全部删除
    public async Task ClearAsync(CancellationToken cancellation = default)
    {
        foreach (var key in await this.Linq(x => x.ToArray()))
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
    #region 内部成员
    #region 读取Cookie
    /// <summary>
    /// 通过JS互操作直接读取document.cookie属性
    /// </summary>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    private ValueTask<string> GetCookie(CancellationToken cancellation = default)
        => JSRuntime.GetProperty<string>("document.cookie", cancellation);
    #endregion
    #region 写入Cookie
    /// <summary>
    /// 通过JS互操作直接写入document.cookie属性
    /// </summary>
    /// <param name="cookie">要写入的Cookie文本</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    private ValueTask SetCookie(string cookie, CancellationToken cancellation = default)
        => SetCookieOriginal($"{cookie};expires=Thu, 01 Jan 2099 00:00:00 GMT", cancellation);
    #endregion
    #region 返回最小UTC时间的字符串
    /// <summary>
    /// 返回JS中UTC最小时间的字符串格式，
    /// 在删除Cookie时会用到
    /// </summary>
    private string MinDate { get; } = "Thu, 01 Jan 1970 00:00:00 GMT";
    #endregion
    #endregion
    #region 构造函数
    /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
    public JSCookie(IJSRuntime jsRuntime)
        : base(jsRuntime)
    {
        IndexAsync = CreateTasks.AsyncIndex<string, string>(
           async (key, cancellation) => (await this.Linq(x => x.First(y => y.Key == key))).Value,
           (key, value, cancellation) => SetCookie($"{key}={value};max-age={3600 * 24 * 365}", cancellation).AsTask());
    }
    #endregion
}
