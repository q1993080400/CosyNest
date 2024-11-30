using System.IOFrancis.FileSystem;
using System.Media;
using System.Text.Json.Serialization;

namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个封装了只读可预览文件的API数据实体
/// </summary>
[JsonDerivedType(typeof(HasReadOnlyPreviewFile), nameof(HasReadOnlyPreviewFile))]
public interface IHasReadOnlyPreviewFile
{
    #region 封面Uri
    /// <summary>
    /// 获取用来预览的封面Uri
    /// </summary>
    string CoverUri { get; }
    #endregion
    #region 本体Uri
    /// <summary>
    /// 获取这个文件本体的Uri
    /// </summary>
    string Uri { get; }
    #endregion
    #region 文件的名称
    /// <summary>
    /// 获取文件的名称
    /// </summary>
    string FileName { get; }
    #endregion
    #region 文件的简称和扩展名
    /// <summary>
    /// 这个属性返回一个元组，
    /// 它的项分别是文件的简称和扩展名（不带点号）
    /// </summary>
    (string Simple, string? Extended) SimpleAndExtended
    {
        get
        {
            var (simple, extended, _) = ToolPath.SplitFilePath(FileName);
            return (simple, extended);
        }
    }
    #endregion
    #region 媒体类型
    /// <summary>
    /// 获取这个文件的媒体类型
    /// </summary>
    MediumFileType MediumFileType
        => ToolMediaType.MediumType(Uri).MediumFileType;
    #endregion
}
