using Amazon.CloudFront;
using Amazon.CloudFront.Model;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个类型是<see cref="IAmazonCloudFrontDistribution"/>的实现，
/// 可以视为一个亚马逊CDN的分配
/// </summary>
/// <param name="eTag">ETag标头，它在更新或删除分配的时候有用</param>
/// <param name="distribution">封装的亚马逊CDN分配，本对象的功能就是通过它实现的</param>
/// <inheritdoc cref="AmazonCloudFrontDistributionBase(AmazonCloudFrontClient)"/>
sealed class AmazonCloudFrontDistribution(AmazonCloudFrontClient amazonCloudFrontClient, string eTag, Distribution distribution) :
    AmazonCloudFrontDistributionBase(amazonCloudFrontClient), IAmazonCloudFrontDistribution
{
    #region 公开成员
    #region ETag标头
    public override string ETag
        => eTag;
    #endregion
    #region ID
    public override string ID
        => distribution.Id;
    #endregion
    #region 域名
    public override string DomainName
        => distribution.DomainName;
    #endregion
    #region 是否启用
    public override bool Enabled
    {
        get => DistributionConfig.Enabled;
        set => DistributionConfig.Enabled = value;
    }
    #endregion
    #region 描述
    public override string Comment
    {
        get => DistributionConfig.Comment;
        set => DistributionConfig.Comment = value;
    }
    #endregion
    #region 枚举所有源草稿的集合
    public override ICollectionFactory<IAmazonCloudFrontOriginDraft> OriginDrafts { get; }
        = CreateCollection.CollectionFactory
        ([.. distribution.DistributionConfig.Origins.Items.Select(x => (IAmazonCloudFrontOriginDraft)new AmazonCloudFrontOrigin(x))], CreateOriginDraft);
    #endregion
    #region 默认缓存行为
    public override ICacheBehavior DefaultCacheBehavior { get; }
        = new CacheBehavior(distribution.DistributionConfig.DefaultCacheBehavior);
    #endregion
    #endregion
    #region 内部成员
    #region 抽象成员实现：获取亚马逊更新请求
    protected override UpdateDistributionRequest GetUpdateRequest()
    {
        var distributionConfig = DistributionConfig;
        return new()
        {
            Id = ID,
            IfMatch = ETag,
            DistributionConfig = new()
            {
                Logging = distributionConfig.Logging,
                Aliases = distributionConfig.Aliases,
                AnycastIpListId = distributionConfig.AnycastIpListId,
                CallerReference = distributionConfig.CallerReference,
                ContinuousDeploymentPolicyId = distributionConfig.ContinuousDeploymentPolicyId,
                DefaultRootObject = distributionConfig.DefaultRootObject,
                CacheBehaviors = distributionConfig.CacheBehaviors,
                Comment = distributionConfig.Comment,
                CustomErrorResponses = distributionConfig.CustomErrorResponses,
                DefaultCacheBehavior = DefaultCacheBehavior.To<CacheBehavior>().Projection(),
                Enabled = distributionConfig.Enabled,
                HttpVersion = distributionConfig.HttpVersion,
                IsIPV6Enabled = distributionConfig.IsIPV6Enabled,
                ViewerCertificate = distributionConfig.ViewerCertificate,
                OriginGroups = distributionConfig.OriginGroups,
                Origins = GetOrigins(),
                PriceClass = distributionConfig.PriceClass,
                Restrictions = distributionConfig.Restrictions,
                WebACLId = distributionConfig.WebACLId,
                Staging = distributionConfig.Staging
            }
        };
    }
    #endregion
    #region 获取分配信息
    /// <summary>
    /// 获取CDN分配的信息
    /// </summary>
    private DistributionConfig DistributionConfig
        => distribution.DistributionConfig;
    #endregion
    #endregion
}
