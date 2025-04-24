using Amazon.CloudFrontKeyValueStore;

namespace System.NetFrancis.Amazon;

public static partial class CreateAmazon
{
    //这个部分类专门用来声明和创建亚马逊KeyValueStore有关的对象的方法

    #region 创建KeyValueStore的API
    /// <summary>
    /// 创建一个可以用来控制亚马逊KeyValueStore的API对象
    /// </summary>
    /// <param name="credentialsInfo">用来登录的凭证</param>
    /// <returns></returns>
    public static IAmazonCloudFrontKeyValueStore AmazonCloudFrontKeyValueStore(AmazonCredentialsInfo credentialsInfo)
    {
        var client = new AmazonCloudFrontKeyValueStoreClient(credentialsInfo.AccessKeyID, credentialsInfo.SecretAccessKey, DefaultRegion);
        return new AmazonCloudFrontKeyValueStore(client);
    }
    #endregion
}
