using Amazon.CloudFront.Model;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个类型是<see cref="IAmazonCloudFrontOrigin"/>的实现，
/// 可以视为一个亚马逊CDN的源
/// </summary>
/// <param name="origin">封装的亚马逊源，本对象的功能就是通过它实现的</param>
sealed class AmazonCloudFrontOrigin(Origin origin) : AmazonCloudFrontOriginBase, IAmazonCloudFrontOrigin
{
    #region ID
    public string ID
        => origin.Id;
    #endregion
    #region 源对应的域名
    public override string DomainName
    {
        get => origin.DomainName;
        set => origin.DomainName = value;
    }
    #endregion
    #region 获取所有自定义标头
    public override IList<CustomHeader> CustomHeaders { get; } = origin.CustomHeaders.FromAmazon();
    #endregion
    #region 抽象成员实现：映射为Origin
    public override Origin Projection()
    {
        origin.CustomHeaders = CustomHeaders.ToAmazon();
        return origin;
    }
    #endregion
}