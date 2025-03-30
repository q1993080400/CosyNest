using Microsoft.AspNetCore;

namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型可以通过JS互操作来索引和修改存储
/// </summary>
/// <param name="jsRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
/// <param name="isLocal">如果这个值为<see langword="true"/>，表示本地存储，否则表示会话存储</param>
sealed class JSStorage(IJSRuntime jsRuntime, bool isLocal) : IBrowserStorage
{
    #region 公开成员
    #region 关于根据键读写值
    #region 读取或写入值（异步索引器）
    private IAsyncIndex<string, string>? IndexAsyncField { get; set; }

    public IAsyncIndex<string, string> IndexAsync
        => IndexAsyncField ??= CreateTask.AsyncIndex<string, string>(
           async (key, cancellation) =>
           (await jsRuntime.InvokeAsync<string?>($"{Prefix}.getItem", cancellation, key)) ??
           throw new KeyNotFoundException($"未找到键{key}"),
           (key, value, cancellation) => jsRuntime.InvokeVoidAsync($"{Prefix}.setItem", cancellation, key, value).AsTask());
    #endregion
    #region 根据键读取值（不会引发异常）
    public async Task<TryGetObject<string>> TryGetValueAsync(string key, CancellationToken cancellation = default)
    {
        var value = await jsRuntime.InvokeAsync<string?>($"{Prefix}.getItem", cancellation, key);
        return new(value is { }, value);
    }
    #endregion
    #endregion
    #region 关于集合
    #region 返回键值对数量
    public async Task<int> CountAsync(CancellationToken cancellation = default)
        => await jsRuntime.GetProperty<int>($"{Prefix}.length", cancellation);
    #endregion
    #region 枚举所有键值对
    public async IAsyncEnumerator<KeyValuePair<string, string>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        for (int count = await CountAsync(cancellationToken), i = 0; i < count; i++)
        {
            var key = await jsRuntime.InvokeAsync<string>($"{Prefix}.key", cancellationToken, i);
            yield return new(key, await IndexAsync.Get(key, cancellationToken));
        }
    }
    #endregion
    #region 关于添加和删除键值对
    #region 删除全部键值对
    public async Task ClearAsync(CancellationToken cancellation = default)
        => await jsRuntime.InvokeVoidAsync($"{Prefix}.clear", cancellation);
    #endregion
    #region 删除指定键
    public async Task<bool> RemoveAsync(string key, CancellationToken cancellation = default)
    {
        if (await this.To<IAsyncDictionary<string, string>>().ContainsKeyAsync(key, cancellation))
        {
            await jsRuntime.InvokeVoidAsync($"{Prefix}.removeItem", cancellation, key);
            return true;
        }
        return false;
    }
    #endregion
    #region 删除指定键值对
    public Task<bool> RemoveAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => RemoveAsync(item.Key, cancellation);
    #endregion
    #region 添加键值对
    public Task AddAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => IndexAsync.Set(item.Key, item.Value, cancellation);
    #endregion
    #endregion
    #region 检查键值对是否存在
    public async Task<bool> ContainsAsync(KeyValuePair<string, string> item, CancellationToken cancellation = default)
        => (await TryGetValueAsync(item.Key, cancellation)) is (true, var value) && Equals(value, item.Value);
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 获取前缀
    /// <summary>
    /// 获取访问存储的前缀
    /// </summary>
    private string Prefix { get; }
        = isLocal ? "localStorage" : "sessionStorage";
    #endregion
    #endregion
}
