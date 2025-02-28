using System.IOFrancis.FileSystem;
using System.Text.Json.Serialization;

namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个封装了只读可预览文件的API数据实体
/// </summary>
[JsonDerivedType(typeof(HasReadOnlyPreviewFile), nameof(HasReadOnlyPreviewFile))]
[JsonDerivedType(typeof(HasPreviewFile), nameof(HasPreviewFile))]
public interface IHasReadOnlyPreviewFile : IWithID
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
    /// 获取文件的名称，
    /// 注意：它指的是文件的可读名称，
    /// 并不是文件在存储层中的实际名称
    /// </summary>
    string FileName { get; }
    #endregion
    #region 文件路径信息
    /// <summary>
    /// 这个属性返回该文件路径的信息
    /// </summary>
    FileNameInfo FilePathInfo
        => FileNameInfo.FromPath(FileName);
    #endregion
    #region 媒体类型
    /// <summary>
    /// 获取这个文件的媒体类型
    /// </summary>
    MediumFileType MediumFileType
        => ToolMediaType.MediumType(FileName).MediumFileType;
    #endregion
    #region 是否启用该文件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示启用这个文件，
    /// 它会被显示或上传
    /// </summary>
    bool IsEnable { get; }
    #endregion
    #region 禁用文件
    /// <summary>
    /// 禁用这个文件，
    /// 它不再被显示或上传
    /// </summary>
    void Disable();
    #endregion
}
