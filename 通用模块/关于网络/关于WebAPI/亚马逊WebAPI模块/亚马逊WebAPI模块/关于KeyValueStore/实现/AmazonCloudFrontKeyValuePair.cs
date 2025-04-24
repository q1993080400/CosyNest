using Amazon.CloudFrontKeyValueStore;
using Amazon.CloudFrontKeyValueStore.Model;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个类型是<see cref="IAmazonCloudFrontKeyValuePair"/>的实现，
/// 可以视为一个亚马逊KeyValueStore的键值对
/// </summary>
/// <param name="listItem">封装的亚马逊KeyValueStore键值对，本对象的功能就是通过它实现的</param>
/// <param name="describe">这个KeyValueStore的元数据</param>
/// <param name="storeClient">封装的亚马逊KeyValueStore客户端对象</param>
sealed class AmazonCloudFrontKeyValuePair(ListKeysResponseListItem listItem, DescribeKeyValueStoreResponse describe, AmazonCloudFrontKeyValueStoreClient storeClient) : IAmazonCloudFrontKeyValuePair
{
    #region ETag标头
    public string ETag
        => describe.ETag;
    #endregion
    #region 键对象
    public string Key
        => listItem.Key;
    #endregion 
    #region 值对象
    public string Value { get; set; } = listItem.Value;
    #endregion
    #region 更新对象
    public async Task Update(CancellationToken cancellationToken = default)
    {
        if (Value == listItem.Value)
            return;
        var request = new PutKeyRequest()
        {
            IfMatch = ETag,
            Key = Key,
            Value = Value,
            KvsARN = describe.KvsARN
        };
        await storeClient.PutKeyAsync(request, cancellationToken);
    }
    #endregion
    #region 删除对象
    public async Task Delete(CancellationToken cancellationToken = default)
    {
        var request = new DeleteKeyRequest()
        {
            IfMatch = ETag,
            Key = Key,
            KvsARN = describe.KvsARN
        };
        await storeClient.DeleteKeyAsync(request, cancellationToken);
    }
    #endregion
}
