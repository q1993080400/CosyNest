namespace System.NetFrancis.Api;

/// <summary>
/// 这个记录封装了必应每日图片的请求结果
/// </summary>
sealed record BingImageDay : IBingImageDay
{
    #region 获取图片Uri
    public required string Uri { get; init; }
    #endregion
}
