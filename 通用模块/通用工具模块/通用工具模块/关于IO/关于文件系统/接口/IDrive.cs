using System.Linq;
using System.Maths;

namespace System.IOFrancis.FileSystem
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个驱动器
    /// </summary>
    public interface IDrive : INode
    {
        #region 返回文件系统
        /// <summary>
        /// 获取文件系统，
        /// 它是所有文件，目录，驱动器的根
        /// </summary>
        new IFileSystem Ancestors
             => this.To<INode>().Ancestors.To<IFileSystem>();
        #endregion
        #region 获取驱动器根文件夹
        /// <summary>
        /// 获取直接隶属于这个驱动器的根文件夹
        /// </summary>
        new IDirectory Son
            => this.To<INode>().Son.First().To<IDirectory>();
        #endregion
        #region 获取名称
        /// <summary>
        /// 获取驱动器名称
        /// </summary>
        string Name { get; }

        /*实现本API请遵循以下规范：
          驱动器名称不要带上点号和斜杠，
          例如C盘的名称应该返回C，而不是C://*/
        #endregion
        #region 获取驱动器格式
        /// <summary>
        /// 获取驱动器文件系统的格式名称
        /// </summary>
        string Format { get; }
        #endregion
        #region 关于容量
        #region 获取总容量
        /// <summary>
        /// 获取驱动器总容量
        /// </summary>
        IUnit<IUTStorage> SizeTotal { get; }
        #endregion
        #region 获取已用容量
        /// <summary>
        /// 获取驱动器已用容量
        /// </summary>
        IUnit<IUTStorage> SizeUsed { get; }
        #endregion
        #region 获取可用容量
        /// <summary>
        /// 获取驱动器可用容量
        /// </summary>
        IUnit<IUTStorage> SizeAvailable
            => SizeTotal - SizeUsed;
        #endregion
        #endregion 
    }
}
