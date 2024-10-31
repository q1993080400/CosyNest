namespace System.IOFrancis.BaseFileSystem;

/// <summary>
/// 该接口为基本文件系统中的目录提供统一抽象
/// </summary>
public interface IDirectoryBase : IIOBase
{
    #region 清除目录
    /// <summary>
    /// 清除目录中的所有子文件和目录，
    /// 但是不将目录本身删除
    /// </summary>
    void Clear()
        => Son.ForEach(x => x.Delete());
    #endregion
    #region 直达子文件或目录
    /// <summary>
    /// 尝试通过相对于本目录的路径，直达子文件或目录
    /// </summary>
    /// <typeparam name="IO">要直达的文件或目录的类型</typeparam>
    /// <param name="relativelyPath">子文件或目录的相对路径，
    /// 它相对于本目录</param>
    /// <returns>直达结果，如果指定的地方不存在文件或目录，则返回默认值</returns>
    IO? Direct<IO>(string relativelyPath)
        where IO : IIOBase;
    #endregion
    #region 创建文件或目录
    #region 通用方法
    /// <summary>
    /// 创建一个文件或目录，并返回
    /// </summary>
    /// <typeparam name="IO">要创建的文件或目录的全名</typeparam>
    /// <param name="name">新文件或目录的名称，
    /// 如果为<see langword="null"/>，则给予一个不重复的随机名称，
    /// 如果已经存在指定的文件或目录，则直接返回</param>
    /// <returns></returns>
    IO Create<IO>(string? name = null)
        where IO : IIOBase;
    #endregion
    #region 创建文件
    /// <summary>
    /// 创建一个文件
    /// </summary>
    /// <typeparam name="File">要创建的文件的类型</typeparam>
    /// <param name="nameSimple">文件的简称，不带扩展名</param>
    /// <param name="nameExtended">文件的扩展名，不带点号</param>
    /// <returns>新创建的文件，如果文件的名称不指定，则分配一个不重复的随机名称，
    /// 如果已经存在指定的文件，则直接返回</returns>
    File CreateFile<File>(string? nameSimple = null, string? nameExtended = null)
        where File : IFileBase
    {
        var name = (nameSimple ?? Guid.NewGuid().ToString()) + (nameExtended is null ? null : $".{nameExtended}");
        return Create<File>(name);
    }
    #endregion
    #endregion
}
