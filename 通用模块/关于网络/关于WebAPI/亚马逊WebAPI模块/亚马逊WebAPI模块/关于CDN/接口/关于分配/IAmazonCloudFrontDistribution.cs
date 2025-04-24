using System.DataFrancis;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个亚马逊CDN的分配，
/// 分配指的是CDN和源之间的单个映射
/// </summary>
public interface IAmazonCloudFrontDistribution : IAmazonCanModifiableObject,
    IAmazonCloudFrontDistributionDraft, IWithID<string>
{
    #region 域名
    /// <summary>
    /// 获取CDN的域名，
    /// 它可以作为网址对外公开
    /// </summary>
    string DomainName { get; }
    #endregion
    #region 获取所有源的集合
    /// <summary>
    /// 获取一个枚举所有源的集合
    /// </summary>
    IEnumerable<IAmazonCloudFrontOrigin> Origins
       => OriginDrafts.OfType<IAmazonCloudFrontOrigin>();
    #endregion
}