namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为亚马逊CDN分配的草稿，
/// 它可以用于添加亚马逊CDN分配
/// </summary>
public interface IAmazonCloudFrontDistributionDraft
{
    #region 是否启用
    /// <summary>
    /// 获取是否启用这个CDN
    /// </summary>
    bool Enabled { get; set; }
    #endregion
    #region 描述
    /// <summary>
    /// 获取对这个分配的描述和说明
    /// </summary>
    string Comment { get; set; }
    #endregion
    #region 获取所有源草稿的集合
    /// <summary>
    /// 获取一个枚举所有源草稿的集合
    /// </summary>
    ICollectionFactory<IAmazonCloudFrontOriginDraft> OriginDrafts { get; }
    #endregion
    #region 默认缓存行为
    /// <summary>
    /// 获取默认缓存行为
    /// </summary>
    ICacheBehavior DefaultCacheBehavior { get; }
    #endregion
}
