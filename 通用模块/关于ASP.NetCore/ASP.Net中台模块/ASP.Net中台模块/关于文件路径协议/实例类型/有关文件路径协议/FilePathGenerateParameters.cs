using System.IOFrancis.FileSystem;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型是文件路径生成的参数
/// </summary>
public sealed record FilePathGenerateParameters
{
    #region 基路径
    /// <summary>
    /// 获取文件的基路径
    /// </summary>
    public required string Path { get; init; }
    #endregion
    #region 文件本来的名字
    /// <summary>
    /// 获取文件的本名，
    /// 也就是它未上传之前本来的名字
    /// </summary>
    public required string TrueName { get; init; }
    #endregion
    #region 文件的名称和扩展名
    /// <summary>
    /// 返回一个元组，它的项分别指示文件的文件名，扩展名（如果有），以及文件的全名
    /// </summary>
    public (string Simple, string? Extended, string FullName) NameAndExtension
        => ToolPath.SplitFilePath(TrueName);
    #endregion
    #region 封面扩展名
    private readonly string? CoverExtensionFiled;

    /// <summary>
    /// 获取封面扩展名，不带点号，
    /// 如果源扩展名是文件，则无需指定它，
    /// 是媒体，且未指定，则与源扩展名相同，
    /// 但是，如果显式指定它，则必须指定一个图片扩展名，否则会引发异常
    /// </summary>
    public string? CoverExtension
    {
        get
        {
            if (CoverExtensionFiled is { })
                return CoverExtensionFiled;
            var extended = NameAndExtension.Extended;
            if (extended is null || FileSource.GetMediaSourceType(extended) is FileSourceType.File)
                return null;
            return extended;
        }
        init
        {
            var ex = value is null ? null : ToolPath.SplitFilePath(value).Extended ?? value;
            if (ex is { } && !FileTypeCom.WebImage.IsCompatible(ex))
                throw new NotSupportedException($"扩展名{ex}不是一个Web兼容的图片文件格式");
            CoverExtensionFiled = ex;
        }
    }
    #endregion
    #region 排序
    /// <summary>
    /// 获取媒体文件的排序，
    /// 排在前面的索引具有更高的优先级
    /// </summary>
    public required IReadOnlyList<int> Sort { get; init; }
    #endregion
}
