using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Maths;

namespace System.IOFrancis.FileSystem
{
    /// <summary>
    /// 这个类型是<see cref="IFile"/>的实现，
    /// 可以被视为一个文件
    /// </summary>
    class FileRealize : IORealize, IFile
    {
#pragma warning disable CA2208

        #region 封装的对象
        #region 封装的文件对象
        /// <summary>
        /// 获取封装的文件对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private new FileInfo PackFS
        {
            get => (FileInfo)base.PackFS;
            set => base.PackFS = value;
        }
        #endregion
        #region 封装本对象的接口形式
        /// <summary>
        /// 获取本对象的接口形式，
        /// 它使本类型的成员可以访问显式实现接口的成员
        /// </summary>
        private IFile File
            => this;
        #endregion
        #endregion
        #region 关于文件的信息
        #region 返回文件的子文件
        public override IEnumerable<INode> Son
            => Enumerable.Empty<INode>();
        #endregion
        #region 获取文件的大小
        public override IUnit<IUTStorage> Size
            => CreateBaseMath.Unit(PackFS.Length, IUTStorage.ByteMetric);
        #endregion
        #endregion
        #region 复制文件
        public override IIO Copy(IDirectory? target, string? newName = null, Func<string, int, string>? rename = null)
        {
            target ??= File.Father ?? throw new ArgumentNullException($"父目录不能为null");
            newName ??= File.NameFull;
            if (rename != null)
            {
                var (simple, extended) = ToolPath.SplitPathFile(newName);
                newName = ToolPath.Distinct(target, newName,
                    (x, y) => ToolPath.GetFullName(rename(simple, y), extended));
            }
            var newPath = IO.Path.Combine(target.Path, newName);
            PackFS.CopyTo(newPath, true);
            return new FileRealize(newPath);
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 用指定的路径初始化文件对象，
        /// 不允许指定不存在的路径
        /// </summary>
        /// <param name="path">指定的路径</param>
        /// <param name="checkExist">在文件不存在的时候，如果这个值为<see langword="true"/>，
        /// 则抛出一个异常，为<see langword="false"/>，则不会抛出异常，而是会创建一个新文件</param>
        public FileRealize(PathText path, bool checkExist = true)
            : base(new FileInfo(path))
        {
            if (!PackFS.Exists)
            {
                if (checkExist)
                    throw ExceptionIO.BecauseExist(path.Path ?? "null");
                PackFS.Create().Dispose();
                PackFS.Refresh();
            }
        }
        #endregion
    }
}
