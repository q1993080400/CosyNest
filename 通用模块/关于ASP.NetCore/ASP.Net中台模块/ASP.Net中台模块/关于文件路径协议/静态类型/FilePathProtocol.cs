using System.Text.RegularExpressions;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 本类型是一个文件路径协议，
/// 它支持按照封面/本体分类路径，以及排序等功能
/// </summary>
static class FilePathProtocol
{
    #region 公开成员
    #region 生成文件路径
    /// <summary>
    /// 根据参数，生成文件路径封装对象
    /// </summary>
    /// <param name="generateParameters">用来生成结果的参数</param>
    /// <returns></returns>
    public static FileSource Generate(FilePathGenerateParameters generateParameters)
    {
        var (_, extended, fullName) = generateParameters.NameAndExtension;
        var sort = generateParameters.Sort.Join("A") + "A";
        var @base = generateParameters.Path;
        var id = Guid.NewGuid().ToString();
        if (extended is null || FileSource.GetMediaSourceType(extended) is FileSourceType.File)
        {
            var filePath = $"{File}{sort}{id}{fullName}";
            return new()
            {
                FilePath = Path.Combine(@base, filePath),
                TrueName = fullName
            };
        }
        var cover = $"{Cover}{sort}{id}M.{generateParameters.CoverExtension}";
        var media = $"{Media}{sort}{id}M.{extended}";
        return new MediaSource()
        {
            CoverPath = Path.Combine(@base, cover),
            FilePath = Path.Combine(@base, media),
            TrueName = fullName
        };
    }

    /*注意：只有非媒体的文件才能够保留原来的名字，
      图片和视频会被强制重命名为M，这是因为：
      图片和视频经常被用来进行其他处理，
      这些处理它们的API不一定考虑到中文路径，带空格的路径等情况*/
    #endregion
    #region 解析文件路径
    #region 正则表达式
    /// <summary>
    /// 这个正则表达式可以用来解析路径
    /// </summary>
    private static IRegex Regex { get; }
        = /*language=regex*/@"(?<type>Media|Cover|File)(?<sort>(\d+A)+)(?<id>[A-Za-z0-9-]{36})(?<name>.+)$".Op().Regex();
    #endregion
    #region 正式方法
    /// <summary>
    /// 解析文件路径，并返回文件源对象
    /// </summary>
    /// <param name="paths">待解析的媒体路径</param>
    /// <returns></returns>
    public static IEnumerable<FileSource> Analysis(IEnumerable<string> paths)
    {
        var bodys = paths.Select(x =>
        {
            var match = Regex.MatcheSingle(x) ??
            throw new ArgumentException($"路径{x}不符合协议的规定，无法对它进行解析");
            return new
            {
                Type = match["type"].Match,
                Sort = match["sort"].Match,
                ID = match["id"].Match,
                Name = match["name"].Match,
                Path = x
            };
        }).ToArray().OrderBy(x => x.Sort, FastRealize.ComparerFromNumbering()).ThenBy(x => x.Name).ToArray().GroupBy(x => x.ID).ToArray();
        foreach (var item in bodys)
        {
            var dictionary = item.ToDictionary(x => x.Type, x => x);
            if (dictionary.TryGetValue(File, out var file))
            {
                yield return new()
                {
                    FilePath = file.Path,
                    TrueName = file.Name,
                };
                continue;
            }
            var media = dictionary[Media];
            yield return new MediaSource()
            {
                CoverPath = dictionary[Cover].Path,
                FilePath = media.Path,
                TrueName = media.Name,
            };
        }
    }
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 获取媒体前缀
    /// <summary>
    /// 获取媒体的前缀
    /// </summary>
    private const string Media = "Media";
    #endregion
    #region 获取封面前缀
    /// <summary>
    /// 获取封面的前缀
    /// </summary>
    private const string Cover = "Cover";
    #endregion
    #region 获取文件前缀
    /// <summary>
    /// 获取文件前缀
    /// </summary>
    private const string File = "File";
    #endregion
    #endregion
}
