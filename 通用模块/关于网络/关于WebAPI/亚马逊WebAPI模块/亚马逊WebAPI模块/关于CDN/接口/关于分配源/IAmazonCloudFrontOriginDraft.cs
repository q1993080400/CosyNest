namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为亚马逊CDN源的草稿，
/// 它可以用于添加亚马逊CDN源
/// </summary>
public interface IAmazonCloudFrontOriginDraft
{
    #region 源对应的域名
    /// <summary>
    /// 获取源所对应的域名
    /// </summary>
    string DomainName { get; set; }
    #endregion
    #region 获取所有自定义标头
    /// <summary>
    /// 获取在请求CDN的时候，
    /// 自动发送到源的所有自定义标头
    /// </summary>
    IList<CustomHeader> CustomHeaders { get; }
    #endregion
}
