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
    #endregion 
}
