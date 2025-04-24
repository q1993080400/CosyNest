using Amazon.CloudFront.Model;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个抽象类是<see cref="IAmazonCloudFrontDistributionDraft"/>的实现，
/// 它是所有亚马逊CDN分配及其草稿的基类
/// </summary>
abstract class AmazonCloudFrontDistributionObjectBase : IAmazonCloudFrontDistributionDraft
{
    #region 公开成员
    #region 是否启用
    public abstract bool Enabled { get; set; }
    #endregion
    #region 描述
    public abstract string Comment { get; set; }
    #endregion
    #region 枚举所有源草稿的集合
    public abstract ICollectionFactory<IAmazonCloudFrontOriginDraft> OriginDrafts { get; }
    #endregion
    #region 默认缓存行为
    public abstract ICacheBehavior DefaultCacheBehavior { get; }
    #endregion
    #endregion 
    #region 内部成员
    #region 创建源对象的草稿
    /// <summary>
    /// 创建一个源对象的草稿
    /// </summary>
    /// <returns></returns>
    protected static IAmazonCloudFrontOriginDraft CreateOriginDraft()
        => new AmazonCloudFrontOriginDraft();
    #endregion
    #region 创建缓存行为
    /// <summary>
    /// 创建一个空白的缓存行为
    /// </summary>
    /// <returns></returns>
    protected static ICacheBehavior CreateCacheBehavior()
        => new CacheBehavior(new());
    #endregion
    #region 获取源
    /// <summary>
    /// 获取描述源的对象
    /// </summary>
    /// <returns></returns>
    protected Origins GetOrigins()
        => new()
        {
            Items = [.. OriginDrafts.Cast<AmazonCloudFrontOriginBase>().Projection()],
            Quantity = OriginDrafts.Count
        };
    #endregion
    #endregion
}
