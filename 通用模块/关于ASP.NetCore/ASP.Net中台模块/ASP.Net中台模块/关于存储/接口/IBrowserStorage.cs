using System.Security.Cryptography;
using System.Text.Json;

using Microsoft.AspNetCore.DataProtection;

namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个浏览器存储对象
/// </summary>
public interface IBrowserStorage : IAsyncDictionary<string, string>
{
    #region 获取对象
    /// <summary>
    /// 从存储中获取值，然后将它反序列化为对象，并返回
    /// </summary>
    /// <typeparam name="Obj">返回值的类型</typeparam>
    /// <param name="key">从存储中获取值的键</param>
    /// <param name="dataProtector">用来加密字符串的对象，
    /// 如果为<see langword="null"/>，则不加密</param>
    /// <param name="options">用来进行Json转换的配置选项</param>
    /// <param name="cancellation">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    async Task<TryGetObject<Obj>> TryGetJsonValueAsync<Obj>(string key, IDataProtector? dataProtector = null,
        JsonSerializerOptions? options = null, CancellationToken cancellation = default)
    {
        var (_, value) = await TryGetValueAsync(key, cancellation);
        if (value.IsVoid())
            return new();
        try
        {
            var jsonText = dataProtector is null ?
                value : dataProtector.Unprotect(value);
            var obj = JsonSerializer.Deserialize<Obj>(jsonText, options);
            return new(true, obj);
        }
        catch (Exception e) when (e is JsonException or CryptographicException)
        {
            await RemoveAsync(key, cancellation);
            return new();
        }
    }
    #endregion
    #region 写入对象
    /// <summary>
    /// 将对象转换为Json字符串，并写入到存储中
    /// </summary>
    /// <typeparam name="Obj">要写入的值的类型</typeparam>
    /// <param name="key">用来从存储中写入值的键</param>
    /// <param name="value">要写入到存储中的值</param>
    /// <returns></returns>
    /// <inheritdoc cref="TryGetJsonValueAsync{Obj}(string, IDataProtector?, JsonSerializerOptions?, CancellationToken)"/>
    async Task SetJsonValueAsync<Obj>(string key, Obj? value, IDataProtector? dataProtector = null,
        JsonSerializerOptions? options = null, CancellationToken cancellation = default)
    {
        var jsonText = JsonSerializer.Serialize(value, options);
        var text = dataProtector is null ?
            jsonText : dataProtector.Protect(jsonText);
        await IndexAsync.Set(key, text, cancellation);
    }
    #endregion
}