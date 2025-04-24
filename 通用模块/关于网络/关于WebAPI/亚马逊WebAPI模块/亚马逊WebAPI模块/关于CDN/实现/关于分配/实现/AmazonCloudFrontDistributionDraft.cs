using System.Design;

using Amazon.CloudFront;
using Amazon.CloudFront.Model;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个类型是<see cref="IAmazonCloudFrontDistributionDraft"/>的实现，
/// 可以视为有一个亚马逊CDN分配的草稿
/// </summary>
sealed class AmazonCloudFrontDistributionDraft : AmazonCloudFrontDistributionObjectBase, IProjection<CreateDistributionRequest>
{
    #region 是否启用
    public override bool Enabled { get; set; } = true;
    #endregion
    #region 描述
    public override string Comment { get; set; } = "默认CDN分配";
    #endregion
    #region 获取所有源草稿的集合
    public override ICollectionFactory<IAmazonCloudFrontOriginDraft> OriginDrafts { get; }
        = CreateCollection.CollectionFactory(CreateOriginDraft);
    #endregion
    #region 默认缓存行为
    public override ICacheBehavior DefaultCacheBehavior { get; }
        = CreateCacheBehavior();
    #endregion
    #region 投影为CreateDistributionRequest
    public CreateDistributionRequest Projection()
    {
        List<string> allowedMethods = ["GET", "HEAD", "OPTIONS", "PUT", "POST", "PATCH", "DELETE"];
        var origins = GetOrigins();
        var functionAssociations = DefaultCacheBehavior.To<CacheBehavior>().
            FunctionAssociations.ToAmazon();
        return new()
        {
            DistributionConfig = new()
            {
                Enabled = Enabled,
                Comment = Comment,
                Origins = origins,
                DefaultCacheBehavior = new()
                {
                    AllowedMethods = new()
                    {
                        Items = allowedMethods,
                        Quantity = allowedMethods.Count
                    },
                    ViewerProtocolPolicy = ViewerProtocolPolicy.RedirectToHttps,
                    Compress = false,
                    TargetOriginId = origins.Items.Single().Id,
                    CachePolicyId = "4cc15a8a-d715-48a4-82b8-cc0b614638fe",
                    OriginRequestPolicyId = "216adef6-5c7f-47e4-b989-5492eafa07d3",
                    FunctionAssociations = functionAssociations
                },
                CallerReference = Guid.CreateVersion7().ToString(),
                HttpVersion = HttpVersion.Http2and3,
                PriceClass = PriceClass.PriceClass_All
            }
        };
    }
    #endregion
}
