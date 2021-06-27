using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Maths;

namespace System.IOFrancis.FileSystem
{
    /// <summary>
    /// 这个类型是<see cref="IFileSystem"/>的实现，
    /// 可以视为一个文件系统
    /// </summary>
    class FileSystemRealize : IFileSystem
    {
        #region 获取父节点
        public INode? Father => null;
        #endregion
        #region 获取子节点
        #region INode版本
        IEnumerable<INode> INode.Son => Son;
        #endregion
        #region IFileSystem版本
        public IEnumerable<IDrive> Son { get; }
        = DriveInfo.GetDrives().Select(x => new DriveRealize(x)).ToArray();
        #endregion
        #endregion
    }
}
