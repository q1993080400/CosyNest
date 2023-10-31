using System.IOFrancis.FileSystem;
using System.NetFrancis;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型可以作为<see cref="RenderVideoJS"/>的媒体源
/// </summary>
public sealed record VideoJSSource
{
    #region Uri
    /// <summary>
    /// 获取这个媒体源的Uri
    /// </summary>
    public required string Uri { get; init; }
    #endregion
    #region 媒体类型
    /// <summary>
    /// 获取这个媒体源的媒体类型
    /// </summary>
    public required string MediumType { get; init; }
    #endregion
    #region 根据Uri创建媒体源
    #region 仅创建一个
    /// <summary>
    /// 根据Uri创建一个媒体源，
    /// 函数会自动根据它的扩展名推断媒体类型
    /// </summary>
    /// <param name="uri">媒体的Uri</param>
    /// <returns></returns>
    public static VideoJSSource CreateSingle(string uri)
    {
        var extended = ToolPath.SplitFilePath(uri).Extended ??
            throw new NotSupportedException($"{uri}不包含扩展名，无法推断它的媒体类型");
        return new()
        {
            Uri = uri,
            MediumType = MediaTypeName.CreateMediumType(extended)
        };
    }
    #endregion
    #region 创建多个
    /// <summary>
    /// 将Uri集合转换为媒体源，
    /// 函数会自动根据它的扩展名推断媒体类型
    /// </summary>
    /// <param name="uris">媒体的Uri集合，
    /// 它的顺序很重要，组件会根据这个依次加载媒体源</param>
    /// <returns></returns>
    public static VideoJSSource[] Create(params string[] uris)
        => uris.Select(CreateSingle).ToArray();
    #endregion
    #endregion
}
