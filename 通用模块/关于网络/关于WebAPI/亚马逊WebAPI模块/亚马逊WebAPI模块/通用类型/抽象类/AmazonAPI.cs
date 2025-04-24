using Amazon.Runtime;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个类型是所有亚马逊服务的基类
/// </summary>
/// <param name="amazonServiceClient">封装的亚马逊服务对象</param>
abstract class AmazonAPI(AmazonServiceClient amazonServiceClient) : IAmazonAPI
{
    #region 释放对象
    public void Dispose()
    {
        amazonServiceClient.Dispose();
    }
    #endregion 
}
