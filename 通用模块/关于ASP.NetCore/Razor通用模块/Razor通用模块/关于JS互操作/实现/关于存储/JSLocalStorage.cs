namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型可以通过JS互操作来索引和修改本地存储
/// </summary>
sealed class JSLocalStorage : JSRuntimeBase, IAsyncDictionary<string, string>
{
    #region 关于根据键读写值
    #region 读取或写入值（异步索引器）
    public IAsyncIndex<string, string> IndexAsync { get; }
    #endregion
    #region 根据键读取值（不会引发异常）
    public async Task<(bool Exist, string? Value)> TryGetValueAsync(string key, CancellationToken cancellation = default)
    {
        var value = await JSRuntime.InvokeAsync<string?>("localStorage.getItem", cancellation, key);
        return (value is { }, value);
    }
    #endregion
    #endregion
    #region 关于集合
    #region 返回键值对数量
    public Task<int> CountAsync(CancellationToken cancellation = default)
        => JSRuntime.GetProperty<int>("localStorage.length", cancellation).AsTask();
    #endregion
    #region 枚举所有键值对
    public async IAsyncEnumerator<KeyValuePair<string, string>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        for (int count = await CountAsync(cancellationToken), i = 0; i < count; i++)
        {
            var key = await JSRuntime.InvokeAsync<string>("localStorage.key", cancellationToken, i);
            yield return new(key, await IndexAsync.Get(key, cancellationToken));
        }
    }
    #endregion
    #region 关于添加和删除键值对
    #region 删除全部键值对
    public Task ClearAsync(CancellationToken cancellation = default)
        => JSRuntime.InvokeVoidAsync("localStorage.clear", cancellation).AsTask();
    #endregion
    #region 删除指定键
    public async Task<bool> RemoveAsync(string key, CancellationToken cancellation = default)
    {
        if (await this.To<IAsyncDictionary<string, string>>().ContainsKeyAsync(key, cancellation))
        {
            await JSRuntime.InvokeVoidAsync("localStorage.removeItem", cancellation, key);
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
    #region 构造函数
    /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
    public JSLocalStorage(IJSRuntime jsRuntime)
        : base(jsRuntime)
    {
        IndexAsync = CreateTasks.AsyncIndex<string, string>(
           async (key, cancellation) =>
           (await jsRuntime.InvokeAsync<string?>("localStorage.getItem", cancellation, key)) ??
           throw new KeyNotFoundException($"未找到键{key}"),
           (key, value, cancellation) => jsRuntime.InvokeVoidAsync("localStorage.setItem", cancellation, key, value).AsTask());
    }
    #endregion
}
