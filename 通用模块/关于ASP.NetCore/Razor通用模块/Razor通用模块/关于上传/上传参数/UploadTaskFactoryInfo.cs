using System.IOFrancis;
using System.Maths;
using System.Maths.Plane;

namespace Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// 这个记录是创建上传任务工厂的参数
/// </summary>
public sealed record UploadTaskFactoryInfo
{
    #region 最大上传大小限制
    /// <summary>
    /// 获取文件的最大上传大小限制
    /// </summary>
    public required IUnit<IUTStorage> MaxLength { get; init; }
    #endregion
    #region 图片封面最大大小
    /// <summary>
    /// 获取图片封面的最大大小，
    /// 它的横纵比并不重要
    /// </summary>
    public required ISizePixel MaxImageCoverSize { get; init; }
    #endregion
    #region 封面格式
    /// <summary>
    /// 获取封面图片的格式
    /// </summary>
    public string CoverFormat { get; init; } = "png";
    #endregion
    #region 最大视频清晰度
    /// <summary>
    /// 获取视频的最大清晰度，
    /// 它的横纵比并不重要
    /// </summary>
    public required ISizePixel MaxDefinition { get; init; }
    #endregion
}
