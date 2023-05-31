using System.IOFrancis.FileSystem;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录封装了媒体的封面和路径
/// </summary>
public sealed record MediaSource
{
    #region 封面Uri
    /// <summary>
    /// 获取媒体封面的Uri，
    /// 封面可以是图片的缩略图，
    /// 也可以是视频的封面
    /// </summary>
    public required string CoverUri { get; init; }
    #endregion
    #region 媒体Uri
    /// <summary>
    /// 获取媒体本体的Uri，
    /// 它可以是图片，视频，也可以是其他文件
    /// </summary>
    public required string MediaUri { get; init; }
    #endregion
    #region 返回返回媒体的类型
    /// <summary>
    /// 返回媒体的类型
    /// </summary>
    public MediaSourceType MediaSourceType => MediaUri switch
    {
        var uri when FileTypeCom.WebImage.IsCompatible(uri) => MediaSourceType.WebImage,
        var uri when FileTypeCom.WebVideo.IsCompatible(uri) => MediaSourceType.WebVideo,
        _ => MediaSourceType.File
    };
    #endregion
}
