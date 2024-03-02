using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;

namespace System.IOFrancis.BaseFileSystem;

/// <summary>
/// 该接口为基本文件系统中的文件提供统一抽象
/// </summary>
public interface IFileBase : IIOBase
{
    #region 关于文件名称
    #region 读写文件的简称
    /// <summary>
    /// 获取或设置文件的简单名称，
    /// 它不带扩展名
    /// </summary>
    string NameSimple { get; set; }
    #endregion
    #region 读写扩展名
    /// <summary>
    /// 当读取这个属性的时候，获取文件的扩展名，不带点号，
    /// 当写入这个属性的时候，修改文件的扩展名，
    /// 如果写入或返回<see cref="string.Empty"/>，代表没有扩展名
    /// </summary>
    string NameExtension { get; set; }
    #endregion
    #region 关于文件类型
    #region 返回文件类型
    /// <summary>
    /// 返回这个文件已注册的文件类型
    /// </summary>
    IEnumerable<IFileType> FileTypes
        => NameExtension switch
        {
            null => [],
            var n => IFileType.RegisteredFileType.
            TryGetValue(n).Value ?? []
        };
    #endregion
    #region 判断文件类型是否兼容
    /// <summary>
    /// 检查这个文件是否与一个文件类型兼容
    /// </summary>
    /// <param name="fileType">要判断的文件类型</param>
    /// <returns>如果兼容，返回<see langword="true"/>，否则返回<see langword="false"/></returns>
    bool IsCompatible(IFileType fileType)
       => fileType.IsCompatible(NameFull);
    #endregion
    #endregion
    #endregion 
    #region 创建数据管道
    /// <summary>
    /// 创建一个可以读写文件的数据管道
    /// </summary>
    /// <returns></returns>
    IFullDuplex GetBitPipe();
    #endregion
}
