namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个亚马逊KeyValueStore的WebAPI，
/// 它是一个可以供函数或其他对象使用的键值对字典
/// </summary>
public interface IAmazonCloudFrontKeyValueStore : IAmazonAPI
{
    #region 获取所有键值对
    /// <summary>
    /// 获取某个KeyValueStore中的所有键值对
    /// </summary>
    /// <param name="kvsARN">键值对所在的KeyValueStore的ARN，它是这个资源的编号</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task<IReadOnlyDictionary<string, IAmazonCloudFrontKeyValuePair>> GetAllKeyValuePair(string kvsARN, CancellationToken cancellationToken = default);
    #endregion
    #region 更新键值对
    /// <summary>
    /// 更新键值对，如果键值对不存在，则会被创建
    /// </summary>
    /// <param name="key">键对象</param>
    /// <param name="value">值对象</param>
    /// <returns></returns>
    /// <inheritdoc cref="GetAllKeyValuePair(string, CancellationToken)"/>
    Task UpdateKeyValue(string kvsARN, string key, string value, CancellationToken cancellationToken = default);
    #endregion
}
