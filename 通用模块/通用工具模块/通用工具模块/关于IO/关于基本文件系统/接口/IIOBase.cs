using System.IOFrancis.FileSystem;
using System.Maths;
using System.Maths.Tree;

namespace System.IOFrancis.BaseFileSystem;

/// <summary>
/// 该接口为基本文件系统提供统一抽象
/// </summary>
public interface IIOBase : INode
{
    #region 关于文件系统树
    #region 获取父目录
    /// <summary>
    /// 当读取这个属性的时候，返回父目录，
    /// 当写入这个属性的时候，将该对象移动到指定的父目录，
    /// 此时如果有同名文件，会将其覆盖
    /// </summary>
    new IDirectoryBase? Father { get; set; }
    #endregion
    #region 枚举直接子文件或目录
    /// <summary>
    /// 获取一个枚举直接子文件或目录的枚举器，
    /// 如果这个类型是文件，则返回一个空集合
    /// </summary>
    new IEnumerable<IIOBase> Son
        => this.To<INode>().Son.Cast<IIOBase>();
    #endregion
    #endregion
    #region 关于路径与名称
    #region 获取完整路径
    /// <summary>
    /// 当读取这个属性时，获取文件或目录的完整路径，
    /// 当写入这个属性时，将文件或目录移动到该路径下，同名文件会被覆盖
    /// </summary>
    string Path { get; set; }
    #endregion 
    #region 读写完整名称
    /// <summary>
    /// 当读取这个属性时，获取当前文件或目录的完整名称
    ///（不是完整路径，如果是文件，带扩展名），
    /// 当写入这个属性时，修改它的名称
    /// </summary>
    string NameFull { get; set; }
    #endregion
    #endregion
    #region 关于文件或目录的大小
    #region 获取文件或目录的大小
    /// <summary>
    /// 获取这个文件或目录的大小
    /// </summary>
    IUnit<IUTStorage> Size { get; }
    #endregion
    #region 返回是否为空文件或目录
    /// <summary>
    /// 返回该对象是否为空文件或空目录
    /// </summary>
    bool IsVoid
        => this is IFile ?
        Size.ValueMetric == 0 : !Son.Any();

    /*问：为什么不直接写Size==0呢？
      这样就可以同时兼容于文件和目录
      答：因为要考虑一种特殊情况，
      那就是目录下的文件全部大小为0，
      在这种情况下，目录的大小也是0，但是它不是空目录*/
    #endregion
    #endregion
    #region 删除
    /// <summary>
    /// 删除这个文件或目录
    /// </summary>
    void Delete();
    #endregion
}
