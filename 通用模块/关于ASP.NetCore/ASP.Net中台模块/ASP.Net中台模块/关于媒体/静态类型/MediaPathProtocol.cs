using System.IOFrancis.FileSystem;
using System.Text.RegularExpressions;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 本类型是一个媒体文件路径协议，
/// 它支持按照封面/本体分类路径，以及排序等功能
/// </summary>
static class MediaPathProtocol
{
    #region 公开成员
    #region 生成文件路径
    /// <summary>
    /// 根据参数，生成媒体路径封装对象
    /// </summary>
    /// <param name="generateParameters">用来生成结果的参数</param>
    /// <returns></returns>
    public static IEnumerable<MediaServerPosition> Generate(IEnumerable<MediaPathGenerateParameters> generateParameters)
        => generateParameters.Select(parameter =>
        {
            var id = Guid.NewGuid().ToString();
            var isImage = parameter.Extension switch
            {
                var uri when FileTypeCom.WebImage.IsCompatible(uri) => true,
                var uri when FileTypeCom.WebVideo.IsCompatible(uri) => false,
                var uri => throw new ArgumentException($"{uri}既不是图片也不是视频")
            };
            var cover = $"{(isImage ? ImageCover : VideoCover)}{parameter.Sort}{id}.{parameter.Extension}";
            var media = $"{(isImage ? Image : Video)}{parameter.Sort}{id}.{parameter.Extension}";
            return new MediaServerPosition()
            {
                CoverPath = cover,
                MediaPath = media
            };
        }).ToArray();
    #endregion
    #region 解析文件路径
    #region 正则表达式
    /// <summary>
    /// 这个正则表达式可以用来解析路径
    /// </summary>
    private static IRegex Regex { get; }
        = /*language=regex*/@"(?<type>ImageCover|Image|VideoCover|Video)(?<sort>\d+)(?<body>[A-Za-z0-9-]{36})".Op().Regex();
    #endregion
    #region 正式方法
    /// <summary>
    /// 解析文件路径，并返回媒体源对象
    /// </summary>
    /// <param name="paths">待解析的媒体路径</param>
    /// <returns></returns>
    public static IEnumerable<MediaSource> Analysis(IEnumerable<string> paths)
    {
        var bodys = paths.Select(x =>
        {
            var match = Regex.MatcheSingle(x) ??
            throw new ArgumentException($"路径{x}不符合协议的规定，无法对它进行解析");
            return new
            {
                Type = match["type"].Match,
                Sort = match["sort"].Match.To<int>(),
                Body = match["body"].Match,
                Path = x
            };
        }).OrderBy(x => x.Sort).GroupBy(x => x.Body).ToArray();
        foreach (var item in bodys)
        {
            var (covers, medias) = item.Split(x => x.Type.EndsWith("Cover"));
            yield return new()
            {
                CoverUri = covers.Single().Path.Op().ToUriPath(),
                MediaUri = medias.Single().Path.Op().ToUriPath()
            };
        }
    }
    #endregion 
    #endregion
    #endregion
    #region 内部成员
    #region 图片封面
    /// <summary>
    /// 获取图片封面的前缀
    /// </summary>
    private static string ImageCover { get; } = "ImageCover";
    #endregion
    #region 图片
    /// <summary>
    /// 获取图片的前缀
    /// </summary>
    private static string Image { get; } = "Image";
    #endregion
    #region 视频封面
    /// <summary>
    /// 获取视频封面的前缀
    /// </summary>
    private static string VideoCover { get; } = "VideoCover";
    #endregion
    #region 视频
    /// <summary>
    /// 获取视频的前缀
    /// </summary>
    private static string Video { get; } = "Video";
    #endregion
    #endregion
}
