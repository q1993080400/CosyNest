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
    /// 获取媒体本体的Uri
    /// </summary>
    public required string MediaUri { get; init; }
    #endregion
    #region 返回媒体是图片还是视频
    /// <summary>
    /// 如果媒体本体是图片，返回<see langword="true"/>，
    /// 是视频，返回<see langword="false"/>，都不是，则引发异常
    /// </summary>
    public bool IsImage => MediaUri switch
    {
        var uri when FileTypeCom.WebImage.IsCompatible(uri) => true,
        var uri when FileTypeCom.WebVideo.IsCompatible(uri) => false,
        var uri => throw new ArgumentException($"{uri}既不是图片也不是视频")
    };
    #endregion
}
