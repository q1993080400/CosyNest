using System.Media;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录表示一个待上传的文件
/// </summary>
public sealed record UploadFileInfo
{
    #region 待上传的文件
    /// <summary>
    /// 获取待上传的文件
    /// </summary>
    public required IBrowserFile File { get; init; }
    #endregion
    #region 预览参数
    /// <summary>
    /// 这个对象可以为预览上传文件提供帮助，
    /// 如果待上传的文件不是图片或者视频，则返回<see langword="null"/>
    /// </summary>
    public required PreviewUploadMediumInfo? PreviewInfo { get; init; }
    #endregion
    #region 文件格式
    /// <summary>
    /// 获取待上传的文件的格式
    /// </summary>
    public MediumFileType MediumFileType
        => CreateMedia.MediumFileType(File.Name);
    #endregion
    #region 是否应该上传该文件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示应该上传这个文件
    /// </summary>
    public bool CanUpload
        => PreviewInfo is null or { IsCancel: false };
    #endregion
}
