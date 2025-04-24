namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个记录是亚马逊CDN的自定义标头
/// </summary>
public sealed record CustomHeader
{
    #region 标头名称
    /// <summary>
    /// 获取标头的名称
    /// </summary>
    public required string HeaderName { get; init; }
    #endregion
    #region 标头的值
    /// <summary>
    /// 获取标头的值
    /// </summary>
    public required string HeaderValue { get; init; }
    #endregion
}
