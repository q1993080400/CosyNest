using Amazon.CloudFrontKeyValueStore;
using Amazon.CloudFrontKeyValueStore.Model;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个类型是<see cref="IAmazonCloudFrontKeyValueStore"/>的实现，
/// 可以用来管理KeyValueStore对象
/// </summary>
/// <param name="storeClient">封装的KeyValueStore客户端对象，本对象的功能就是通过它实现的</param>
sealed class AmazonCloudFrontKeyValueStore(AmazonCloudFrontKeyValueStoreClient storeClient) :
    AmazonAPI(storeClient), IAmazonCloudFrontKeyValueStore
{
    #region 公开成员
    #region 获取所有键值对
    public async Task<IReadOnlyDictionary<string, IAmazonCloudFrontKeyValuePair>> GetAllKeyValuePair(string kvsARN, CancellationToken cancellationToken = default)
    {
        var responseDescribe = await GetDescribe(kvsARN, cancellationToken);
        var request = new ListKeysRequest()
        {
            KvsARN = kvsARN,
            MaxResults = 50
        };
        var response = await storeClient.ListKeysAsync(request, cancellationToken);
        return response.Items.Select(x => (IAmazonCloudFrontKeyValuePair)new AmazonCloudFrontKeyValuePair(x, responseDescribe, storeClient)).
            ToDictionary(x => x.Key);
    }
    #endregion
    #region 更新键值对
    public async Task UpdateKeyValue(string kvsARN, string key, string value, CancellationToken cancellationToken = default)
    {
        var responseDescribe = await GetDescribe(kvsARN, cancellationToken);
        var request = new PutKeyRequest()
        {
            Key = key,
            Value = value,
            KvsARN = kvsARN,
            IfMatch = responseDescribe.ETag
        };
        await storeClient.PutKeyAsync(request, cancellationToken);
    }
    #endregion
    #endregion 
    #region 内部成员
    #region 返回存储的元数据
    /// <summary>
    /// 返回这个KeyValueStore的元数据
    /// </summary>
    /// <param name="kvsARN">这个KeyValueStore的ARN</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    private async Task<DescribeKeyValueStoreResponse> GetDescribe(string kvsARN, CancellationToken cancellationToken = default)
    {
        var requestDescribe = new DescribeKeyValueStoreRequest()
        {
            KvsARN = kvsARN
        };
        return await storeClient.DescribeKeyValueStoreAsync(requestDescribe, cancellationToken);
    }
    #endregion
    #endregion
}
