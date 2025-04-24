using Amazon.CloudFront.Model;

using System.Design;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个抽象类是<see cref="IAmazonCloudFrontOriginDraft"/>的实现，
/// 可以视为一个亚马逊CDN源的草稿
/// </summary>
abstract class AmazonCloudFrontOriginBase : IAmazonCloudFrontOriginDraft, IProjection<Origin>
{
    #region 源对应的域名
    public abstract string DomainName { get; set; }
    #endregion
    #region 获取所有自定义标头
    public abstract IList<CustomHeader> CustomHeaders { get; }
    #endregion
    #region 抽象成员：映射为Origin
    public abstract Origin Projection();
    #endregion 
}