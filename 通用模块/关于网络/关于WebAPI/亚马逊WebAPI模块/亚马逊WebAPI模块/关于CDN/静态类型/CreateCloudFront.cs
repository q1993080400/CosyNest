using Amazon.CloudFront;

namespace System.NetFrancis.Amazon;

public static partial class CreateAmazon
{
    //这个部分类专门用来声明和创建亚马逊CDN有关的对象的方法

    #region 创建CDN的API
    /// <summary>
    /// 创建一个可以用来控制亚马逊CDN的API对象
    /// </summary>
    /// <param name="credentialsInfo">用来登录的凭证</param>
    /// <returns></returns>
    public static IAmazonCloudFront AmazonCloudFront(AmazonCredentialsInfo credentialsInfo)
    {
        var client = new AmazonCloudFrontClient(credentialsInfo.AccessKeyID, credentialsInfo.SecretAccessKey, DefaultRegion);
        return new AmazonCloudFront(client);
    }
    #endregion
}
