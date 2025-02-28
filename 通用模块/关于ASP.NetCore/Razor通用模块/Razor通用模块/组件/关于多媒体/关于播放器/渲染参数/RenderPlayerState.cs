using System.IOFrancis.FileSystem;
using System.Net;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="Player"/>的状态
/// </summary>
public sealed record RenderPlayerState
{
    #region 媒体的ID
    /// <summary>
    /// 获取媒体的ID，
    /// 它必须被赋值给指定的video或audio标签
    /// </summary>
    public required string PlayerID { get; init; }
    #endregion
    #region 媒体的长度
    /// <summary>
    /// 获取媒体的长度，以秒为单位，
    /// 如果为<see langword="null"/>，表示未知
    /// </summary>
    public double? Length { get; init; }
    #endregion
    #region 是否正在播放
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示处于播放状态，否则处于暂停或未播放状态
    /// </summary>
    public bool InPlay { get; init; }
    #endregion
    #region 正在播放的媒体名称
    #region 正式属性
    private string? MediaNameFiled { get; init; }

    /// <summary>
    /// 获取正在播放的媒体名称，
    /// 它不包含扩展名，
    /// 如果没有播放，则为<see langword="null"/>
    /// </summary>
    public string? MediaName
    {
        get => GetMediaName(MediaNameFiled);
        init => MediaNameFiled = value;
    }
    #endregion
    #region 内部成员：获取媒体名称
    /// <summary>
    /// 获取媒体名称，
    /// 如果它为<see langword="null"/>，
    /// 尝试自动获取
    /// </summary>
    /// <param name="mediaName">输入的媒体名称</param>
    /// <returns></returns>
    private string? GetMediaName(string? mediaName)
    {
        var newMediaName = mediaName switch
        {
            var name when name.IsVoid()
            => RenderPlayerStateOperational.MediumSource switch
            {
            [{ } uri, ..] => uri,
                _ => null
            },
            var name => name
        };
        return newMediaName is null ?
            null :
            FileNameInfo.FromPath(WebUtility.UrlDecode(newMediaName)).Simple;
    }
    #endregion
    #endregion
    #region 允许用户控制的部分
    /// <summary>
    /// 获取渲染参数中允许用户控制的部分
    /// </summary>
    public required RenderPlayerStateOperational RenderPlayerStateOperational { get; init; }
    #endregion
}
