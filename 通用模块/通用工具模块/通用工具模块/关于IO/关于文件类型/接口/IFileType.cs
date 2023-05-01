using System.Collections.Concurrent;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个文件类型
/// </summary>
public interface IFileType
{
    #region 重载+运算符
    #region 传入扩展名集合
    /// <summary>
    /// 合并文件类型
    /// </summary>
    /// <param name="fileType">待合并的文件类型</param>
    /// <param name="extend">新增支持的扩展名</param>
    /// <returns>一个新的<see cref="IFileType"/>，
    /// 它同时支持<paramref name="fileType"/>和<paramref name="extend"/>所声明的文件类型</returns>
    public static IFileType operator +(IFileType fileType, IEnumerable<string> extend)
        => fileType.Merge(extend);
    #endregion
    #region 传入另一个文件类型
    /// <summary>
    /// 合并文件类型
    /// </summary>
    /// <param name="fileType">待合并的文件类型</param>
    /// <param name="fileTypeB">新增支持的文件类型</param>
    /// <returns>一个新的<see cref="IFileType"/>，
    /// 它同时支持<paramref name="fileType"/>和<paramref name="fileTypeB"/>所声明的文件类型</returns>
    public static IFileType operator +(IFileType fileType, IFileType fileTypeB)
        => fileType.Merge(fileTypeB);
    #endregion
    #endregion
    #region 已注册的文件类型
    #region 公开属性
    /// <summary>
    /// 返回已注册的文件类型，键是扩展名，值是文件类型，
    /// 在注册之后，文件类型可以被<see cref="IFile"/>识别
    /// </summary>
    public static IReadOnlyDictionary<string, IEnumerable<IFileType>> RegisteredFileType
        => RegisteredFileTypePR.ToDictionary(x => x.Key, x => (IEnumerable<IFileType>)x.Value);
    #endregion
    #region 私有属性
    /// <summary>
    /// 返回已注册的文件类型，键是扩展名，值是文件类型，
    /// 在注册之后，文件类型可以被<see cref="IFile"/>识别
    /// </summary>
    protected static IAddOnlyDictionary<string, HashSet<IFileType>> RegisteredFileTypePR { get; }
    = new ConcurrentDictionary<string, HashSet<IFileType>>().FitDictionary(false);

    /*注释：文件类型使用集合的原因在于：
       同一个扩展名可能对应多个文件类型，例如：
       exe既是程序集文件，也是可执行文件*/
    #endregion
    #endregion
    #region 对文件类型的说明
    /// <summary>
    /// 对文件类型的说明
    /// </summary>
    string Description { get; }
    #endregion
    #region 受支持的扩展名
    /// <summary>
    /// 表示属于这个文件类型的扩展名，不带点号
    /// </summary>
    IEnumerable<string> ExtensionName { get; }
    #endregion
    #region 合并文件类型
    #region 传入扩展名集合
    /// <summary>
    /// 合并文件类型
    /// </summary>
    /// <param name="fileType">新增支持的文件类型的扩展名</param>
    /// <param name="description">对新文件类型的描述</param>
    /// <returns>一个新的<see cref="IFileType"/>对象，
    /// 它同时支持本对象和<paramref name="fileType"/>中所声明的文件类型</returns>
    IFileType Merge(IEnumerable<string> fileType, string description = "");
    #endregion
    #region 传入另一个文件类型
    /// <summary>
    /// 合并文件类型
    /// </summary>
    /// <param name="fileType">新增支持的文件类型</param>
    /// <param name="description">对新文件类型的描述</param>
    /// <returns>一个新的<see cref="IFileType"/>对象，
    /// 它同时支持本对象和<paramref name="fileType"/>中所声明的文件类型</returns>
    IFileType Merge(IFileType fileType, string description = "")
        => Merge(fileType.ExtensionName, description);
    #endregion
    #endregion
    #region 检查文件类型兼容性
    #region 传入扩展名或路径
    /// <summary>
    /// 检查扩展名或路径与文件类型是否兼容
    /// </summary>
    /// <param name="extensionNameOrPath">指定的扩展名或路径，
    /// 如果它是扩展名，则不带点号</param>
    bool IsCompatible(string extensionNameOrPath)
    {
        var extensionName = ToolPath.SplitPathFile(extensionNameOrPath).Extended ?? extensionNameOrPath;
        return ExtensionName.Contains(extensionName);
    }
    #endregion
    #region 传入文件对象
    /// <summary>
    /// 检查文件是否与文件类型兼容
    /// </summary>
    /// <param name="file">待检查的文件</param>
    /// <returns></returns>
    bool IsCompatible(IFile file)
        => IsCompatible(file.NameFull);
    #endregion
    #region 传入另一个文件类型
    /// <summary>
    /// 检查另一个文件类型是否与本文件类型兼容
    /// </summary>
    /// <param name="fileTypeB">要判断的文件类型B</param>
    bool IsCompatible(IFileType fileTypeB)
       => fileTypeB.ExtensionName.IsSupersetOf(ExtensionName, true).IsSubset();
    #endregion
    #endregion
}
