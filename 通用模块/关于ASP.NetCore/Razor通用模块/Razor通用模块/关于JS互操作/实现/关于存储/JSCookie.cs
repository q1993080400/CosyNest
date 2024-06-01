using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

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
    public async ValueTask SetCookieOriginal(string cookie, CancellationToken cancellation = default)
    {
        await JSRuntime.SetProperty("document.cookie", cookie, cancellation);
        await Refresh(cancellation);
    }
    #endregion
    #region 读取或写入值（异步索引器）
    public IAsyncIndex<string, string> IndexAsync { get; }
    #endregion
    #region 读取Cookie且不引发异常
    public async Task<(bool Exist, string? Value)> TryGetValueAsync(string key, CancellationToken cancellation = default)
    {
        var exist = (await Cookie).TryGetValue(key, out var value);
        return (exist, value);
    }
    #endregion
    #endregion
    #region 关于集合
    #region Cookie缓存
    private ImmutableDictionary<string, string> CookieField { get; set; }

    /// <summary>
    /// 获取Cookie的缓存
    /// </summary>
    private Task<ImmutableDictionary<string, string>> Cookie
    {
        get
        {
            #region 本地函数
            async Task<ImmutableDictionary<string, string>> Fun()
            {
                if (CookieField is null)
                    await Refresh();
                return CookieField;
            }
            #endregion
            return Fun();
        }
    }
    #endregion
    #region 枚举所有键值对
    #region 正式属性
    public async IAsyncEnumerator<KeyValuePair<string, string>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        var cookie = await Cookie;
        foreach (var item in cookie)
        {
            yield return item;
        }
    }
    #endregion
    #region 辅助类型
    private record KeyAndValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    #endregion
    #endregion
    #region 检查键值对是否存在
    public async Task<bool> ContainsAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => (await Cookie).ContainsKey(item.Key);
    #endregion
    #endregion
    #region 关于添加和删除键值对
    #region 删除指定的键
    public async Task<bool> RemoveAsync(string key, CancellationToken cancellation = default)
    {
        var remove = await JSRuntime.InvokeAsync<bool>("docCookies.removeItem", cancellation, key, "/");
        CookieField = (await Cookie).Remove(key);
        return remove;
    }
    #endregion
    #region 删除指定的键值对
    public async Task<bool> RemoveAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => await RemoveAsync(item.Key, cancellation);
    #endregion
    #region 全部删除
    public async Task ClearAsync(CancellationToken cancellation = default)
    {
        await JSRuntime.InvokeVoidAsync("docCookies.clear", cancellation, "/");
        CookieField = (await Cookie).Clear();
    }
    #endregion
    #region 添加键值对
    public async Task AddAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
    {
        await IndexAsync.Set(item.Key, item.Value, cancellation);
    }
    #endregion
    #endregion
    #region 刷新Cookie
    [MemberNotNull(nameof(CookieField))]
    public async Task Refresh(CancellationToken cancellationToken = default)
    {
#pragma warning disable CS8774
        var cookies = await JSRuntime.InvokeAsync<KeyAndValue[]>("docCookies.keyAndValue", cancellationToken);
        var range = cookies.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToArray();
        CookieField = ImmutableDictionary<string, string>.Empty.AddRange(range);
#pragma warning restore
    }
    #endregion
    #endregion
    #region 构造函数
    /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
    public JSCookie(IJSRuntime jsRuntime)
        : base(jsRuntime)
    {
        IndexAsync = CreateTasks.AsyncIndex<string, string>(
           async (key, cancellation) =>
           {
               var (_, value) = await TryGetValueAsync(key, cancellation);
               return value ?? "";
           },
           async (key, value, cancellation) =>
           {
               await JSRuntime.InvokeVoidAsync("docCookies.setItem", cancellation, key, value, 3600 * 24 * 90, "/");
               CookieField = (await Cookie).SetItem(key, value);
           });
    }
    #endregion
}
