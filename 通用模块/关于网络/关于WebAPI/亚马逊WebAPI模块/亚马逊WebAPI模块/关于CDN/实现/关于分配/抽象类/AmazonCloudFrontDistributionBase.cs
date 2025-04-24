using Amazon.CloudFront.Model;
using Amazon.CloudFront;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个类型是<see cref="IAmazonCloudFrontDistribution"/>的实现，
/// 它是所有亚马逊CDN分配的基类
/// </summary>
/// <param name="amazonCloudFrontClient">封装的亚马逊CDN客户端，本对象的功能就是通过它实现的</param>
abstract class AmazonCloudFrontDistributionBase(AmazonCloudFrontClient amazonCloudFrontClient) : AmazonCloudFrontDistributionObjectBase, IAmazonCloudFrontDistribution
{
    #region 公开成员
    #region ETag标头
    public abstract string ETag { get; }
    #endregion
    #region ID
    public abstract string ID { get; }
    #endregion
    #region 域名
    public abstract string DomainName { get; }
    #endregion
    #region 修改对象
    public async Task Update(CancellationToken cancellationToken = default)
    {
        var request = GetUpdateRequest();
        await amazonCloudFrontClient.UpdateDistributionAsync(request, cancellationToken);
    }
    #endregion
    #region 删除对象
    public async Task Delete(CancellationToken cancellationToken = default)
    {
        if (Enabled)
            throw new NotSupportedException("请先禁用分配，然后再删除它");
        var deleteDistributionRequest = new DeleteDistributionRequest()
        {
            Id = ID,
            IfMatch = ETag
        };
        await amazonCloudFrontClient.DeleteDistributionAsync(deleteDistributionRequest, cancellationToken);
    }
    #endregion
    #endregion 
    #region 内部成员
    #region 抽象成员：获取亚马逊更新请求
    /// <summary>
    /// 获取一个用于更新亚马逊CDN分配的请求
    /// </summary>
    /// <returns></returns>
    protected abstract UpdateDistributionRequest GetUpdateRequest();
    #endregion
    #endregion
}
