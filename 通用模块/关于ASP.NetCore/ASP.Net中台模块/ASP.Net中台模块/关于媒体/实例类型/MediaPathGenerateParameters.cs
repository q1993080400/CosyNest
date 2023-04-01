using System.IOFrancis.FileSystem;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是媒体路径生成的参数
/// </summary>
public sealed record MediaPathGenerateParameters
{
    #region 基路径
    /// <summary>
    /// 获取媒体文件的基路径
    /// </summary>
    public required string Path { get; init; }
    #endregion
    #region 扩展名
    /// <summary>
    /// 获取媒体文件的扩展名，不带点号
    /// </summary>
    public required string Extension { get; init; }
    #endregion
    #region 封面扩展名
    private readonly string? CoverExtensionField;

    /// <summary>
    /// 获取封面扩展名，如果不指定，
    /// 则与<see cref="Extension"/>相同，
    /// 但是如果扩展名不是兼容的Web图片格式，会引发异常
    /// </summary>
    public string CoverExtension
    {
        get
        {
            var extension = CoverExtensionField ?? Extension;
            return FileTypeCom.WebImage.IsCompatible(extension) ?
                extension :
                throw new ArgumentException($"{extension}不是兼容的Web图片格式");
        }
        init => CoverExtensionField = value;
    }
    #endregion
    #region 排序
    /// <summary>
    /// 获取媒体文件的排序，顺序为升序
    /// </summary>
    public required int Sort { get; init; }
    #endregion
}
