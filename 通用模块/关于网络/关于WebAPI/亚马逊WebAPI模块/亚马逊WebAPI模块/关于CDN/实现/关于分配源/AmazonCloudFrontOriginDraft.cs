using Amazon.CloudFront;
using Amazon.CloudFront.Model;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个类型是<see cref="IAmazonCloudFrontOriginDraft"/>的实现，
/// 可以视为一个亚马逊CDN源的草稿
/// </summary>
sealed class AmazonCloudFrontOriginDraft : AmazonCloudFrontOriginBase
{
    #region 源对应的域名
    public override string DomainName { get; set; } = "";
    #endregion
    #region 获取所有自定义标头
    public override IList<CustomHeader> CustomHeaders { get; } = [];
    #endregion
    #region 抽象成员实现：映射为Origin
    public override Origin Projection()
    {
        List<string> originSslProtocolsItems = ["TLSv1.2"];
        return new()
        {
            Id = DomainName,
            DomainName = DomainName,
            CustomHeaders = CustomHeaders.ToAmazon(),
            CustomOriginConfig = new()
            {
                OriginSslProtocols = new()
                {
                    Items = originSslProtocolsItems,
                    Quantity = originSslProtocolsItems.Count
                },
                OriginProtocolPolicy = OriginProtocolPolicy.HttpsOnly,
                HTTPPort = 80,
                HTTPSPort = 443,
            }
        };
    }
    #endregion
}