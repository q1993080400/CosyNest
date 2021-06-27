using System.Collections.Generic;
using System.IO;
using System.Maths;

namespace System.IOFrancis.FileSystem
{
    /// <summary>
    /// 这个类型是文件和目录接口实现的共同基类
    /// </summary>
    abstract class IORealize : IIO
    {
#pragma warning disable CA2208

        #region 封装的对象
        /// <summary>
        /// 获取封装的文件目录对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        protected FileSystemInfo PackFS { get; set; }
        #endregion
        #region 关于文件系统树
        #region 返回根节点
        INode INode.Ancestors
            => CreateIO.FileSystem;
        #endregion
        #region 获取父目录
        #region INode版本
        INode? INode.Father
            => Father ?? (INode)Drive;
        #endregion
        #region IIO版本
        public IDirectory? Father
        {
            get
            {
                var father = IO.Path.GetDirectoryName(Path)!;
                return father.IsVoid() ? null : CreateIO.Directory(father);
            }
            set
            {
                if (value is null)
                    throw new ArgumentNullException($"{nameof(Father)}不能写入null值");
                Path = IO.Path.Combine(value.Path, this.To<IIO>().NameFull);
            }
        }
        #endregion
        #endregion
        #region 枚举直接子文件或子目录
        public abstract IEnumerable<INode> Son { get; }
        #endregion
        #region 获取文件或目录的驱动器
        public IDrive Drive
            => CreateIO.FileSystem[Path[0].ToString()]!;
        #endregion
        #endregion
        #region 文件或目录的信息
        #region 完整路径
        public string Path
        {
            get => PackFS.FullName;
            set => PackFS.To<dynamic>().
                MoveTo(value ?? throw new ArgumentNullException($"{nameof(Path)}不能写入null值"));
        }
        #endregion
        #region 获取文件或目录的大小
        public abstract IUnit<IUTStorage> Size { get; }
        #endregion
        #region 是否隐藏文件或目录
        public bool Hide
        {
            get => PackFS.Attributes.HasFlag(FileAttributes.Hidden);
            set => PackFS.Attributes = value ?
                PackFS.Attributes | FileAttributes.Hidden :
                PackFS.Attributes.RemoveFlag(FileAttributes.Hidden);
        }
        #endregion
        #endregion
        #region 对文件或目录的操作
        #region 删除
        public void Delete()
        {
            if (PackFS is DirectoryInfo d)
                d.Delete(true);
            else PackFS.Delete();
        }
        #endregion
        #region 复制
        public abstract IIO Copy(IDirectory? target, string? newName = null, Func<string, int, string>? rename = null);
        #endregion
        #region 对文件或目录执行原子操作
        public Ret Atomic<Ret>(Func<IIO, Ret> @delegate)
        {
            IIO iO = this;
            var isVoid = iO.IsVoid;
            var backup = isVoid ? this : Copy(iO.Father!, Guid.NewGuid().ToString());         //如果文件或目录为空，则不进行备份，因为它很可能是新创建的文件
            Ret r;
            try
            {
                r = @delegate(this);                                  //对原始文件，而不是备份执行操作
            }
            catch (Exception)
            {
                Delete();                                           //如果出现异常，则删除自身
                if (!isVoid)                                        //如果做了备份，则将备份转正
                {
                    backup.Path = Path;
                    PackFS = this is IFile ?
                        new FileInfo(backup.Path) : new DirectoryInfo(backup.Path);
                }
                throw;
            }
            if (!isVoid)                                //在做了备份的情况下，如果没有出现异常，则还会删除备份
                backup.Delete();
            return r;
        }
        #endregion
        #region 手动刷新文件或目录的状态
        public void Refresh()
            => PackFS.Refresh();
        #endregion
        #endregion
        #region 重写的方法
        #region 重写的GetHashCode
        public override int GetHashCode()
            => Path.GetHashCode();
        #endregion
        #region 重写Equals
        public override bool Equals(object? obj)
            => obj is IIO i &&
            Path == i.Path;
        #endregion
        #region 重写ToString
        public override string ToString()
            => Path;
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的文件目录对象初始化对象
        /// </summary>
        /// <param name="packFS">指定的文件目录对象，
        /// 本对象的功能就是通过它实现的</param>
        public IORealize(FileSystemInfo packFS)
        {
            this.PackFS = packFS;
        }
        #endregion
    }
}
