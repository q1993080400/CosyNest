using System.MathFrancis.Plane;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录是上传媒体时中间件的参数
/// </summary>
public sealed class UploadMiddlewareMediumInfo
{
    #region 图片封面最大大小
    /// <summary>
    /// 获取图片封面的最大大小，
    /// 它的横纵比并不重要
    /// </summary>
    public required ISizePixel MaxImageCoverSize { get; init; }
    #endregion
    #region 最大视频清晰度
    /// <summary>
    /// 获取视频的最大清晰度，
    /// 它的横纵比并不重要，
    /// 如果为<see langword="null"/>，表示不限制
    /// </summary>
    public ISizePixel? MaxDefinition { get; init; }
    #endregion
    #region 封面格式
    /// <summary>
    /// 获取封面图片的格式
    /// </summary>
    public string CoverFormat { get; init; } = "webp";
    #endregion
}
