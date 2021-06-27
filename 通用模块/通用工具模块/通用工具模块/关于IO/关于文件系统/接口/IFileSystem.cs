using System.Collections.Generic;
using System.Linq;
using System.Maths;

namespace System.IOFrancis.FileSystem
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个文件系统，
    /// 它是所有文件，目录和驱动器的根
    /// </summary>
    public interface IFileSystem : INode
    {
        #region 枚举所有驱动器
        /// <summary>
        /// 获取一个枚举所有驱动器的枚举器
        /// </summary>
        new IEnumerable<IDrive> Son { get; }
        #endregion
        #region 根据名称获取驱动器
        /// <summary>
        /// 根据名称获取驱动器
        /// </summary>
        /// <param name="name">要寻找的驱动器的名称</param>
        /// <returns>具有指定名称的驱动器，如果没有找到，则返回<see langword="null"/></returns>
        IDrive? this[string name]
            => Son.FirstOrDefault(x => x.Name == name);
        #endregion
        #region 关于容量
        #region 辅助方法
        /// <summary>
        /// 计算容量的辅助方法
        /// </summary>
        /// <param name="delegate">通过驱动器获取容量的委托</param>
        /// <returns></returns>
        private IUnit<IUTStorage> SizeAided(Func<IDrive, IUnit<IUTStorage>> @delegate)
            => Son.Select(@delegate).Sum();
        #endregion
        #region 获取总容量
        /// <summary>
        /// 获取文件系统总容量
        /// </summary>
        IUnit<IUTStorage> SizeTotal
            => SizeAided(x => x.SizeTotal);
        #endregion
        #region 获取已用容量
        /// <summary>
        /// 获取文件系统已用容量
        /// </summary>
        IUnit<IUTStorage> SizeUsed
            => SizeAided(x => x.SizeUsed);
        #endregion
        #region 获取可用容量
        /// <summary>
        /// 获取文件系统可用容量
        /// </summary>
        IUnit<IUTStorage> SizeAvailable
            => SizeAided(x => x.SizeAvailable);
        #endregion
        #endregion 
    }
}
