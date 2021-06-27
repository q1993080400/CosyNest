using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Maths;

namespace System.IOFrancis.FileSystem
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个文件或目录
    /// </summary>
    public interface IIO : INode
    {
        #region 说明文档
        /*在实现本接口时，请遵循以下规范：
          #在初始化本对象的时候， 如果文件或目录不存在，则应当抛出异常，
          禁止初始化不存在的文件或目录，这是本接口的一个约定

          #在返回子文件和子目录的时候，应该使用迭代器延迟返回，
          如果需要遍历整个文件系统，这样可以大大改善性能*/
        #endregion
        #region 关于文件系统树
        #region 返回根节点
        /// <summary>
        /// 返回文件系统，
        /// 它是所有文件，目录和驱动器的根
        /// </summary>
        new IFileSystem Ancestors
             => (IFileSystem)this.To<INode>().Ancestors;
        #endregion
        #region 读写文件的父目录
        /// <summary>
        /// 当读取这个属性的时候，返回父目录，
        /// 当写入这个属性的时候，将该对象移动到指定的父目录，
        /// 此时如果有同名文件，会将其覆盖
        /// </summary>
        new IDirectory? Father { get; set; }

        /*实现本API请遵循以下规范：
          如果该对象是目录，而且是所在驱动器的根目录，则这个属性应返回null，
          但是在这种情况下,INode.Father应返回该目录的驱动器，
          这是因为驱动器不是IDirectory，但仍然是文件系统中的一个节点*/
        #endregion
        #region 获取文件或目录的驱动器
        /// <summary>
        /// 获取这个文件或目录所在的驱动器
        /// </summary>
        IDrive Drive { get; }
        #endregion
        #region 枚举直接子文件或目录
        /// <summary>
        /// 获取一个枚举直接子文件或目录的枚举器，
        /// 如果这个类型是文件，则返回一个空集合
        /// </summary>
        new IEnumerable<IIO> Son
            => this.To<INode>().Son.Cast<IIO>();
        #endregion
        #endregion
        #region 文件或目录的信息
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
        #region 是否隐藏文件或目录
        /// <summary>
        /// 如果这个值为<see langword="true"/>，
        /// 代表该文件或目录为隐藏，否则代表正常显示
        /// </summary>
        bool Hide { get; set; }
        #endregion
        #region 关于路径与名称
        #region 读写完整路径
        /// <summary>
        /// 当读取这个属性时，获取文件或目录的完整路径，
        /// 当写入这个属性时，将文件或目录移动到该路径下
        /// </summary>
        string Path { get; set; }
        #endregion
        #region 读写完整名称
        /// <summary>
        /// 当读取这个属性时，获取当前文件或目录的完整名称
        ///（不是完整路径，如果是文件，带扩展名），
        /// 当写入这个属性时，修改它的名称
        /// </summary>
        string NameFull
        {
            get => IO.Path.GetFileName(Path);
            set
            {
#pragma warning disable CA2208
                var father = IO.Path.GetDirectoryName(Path)!;
                Path = IO.Path.Combine(father, value ?? throw new ArgumentNullException($"{nameof(NameFull)}禁止写入null值"));
#pragma warning restore
            }
        }
        #endregion
        #endregion
        #endregion
        #region 对文件或目录的操作
        #region 删除
        /// <summary>
        /// 删除这个文件或目录
        /// </summary>
        void Delete();
        #endregion
        #region 复制
        /// <summary>
        /// 将这个文件或目录复制到新目录，
        /// 如果这个对象是目录，还会递归复制所有子目录
        /// </summary>
        /// <param name="target">复制的目标目录，
        /// 如果为<see langword="null"/>，则默认复制到当前目录</param>
        /// <param name="newName">这个参数允许在复制的同时修改文件或目录的名称（指全名），
        /// 如果为<see langword="null"/>，代表不改名</param>
        /// <param name="rename">当复制的目标目录存在同名文件或目录时，
        /// 如果这个值为<see langword="null"/>，则覆盖同名文件或合并同名目录，
        /// 否则执行该委托赋予一个新的名称，委托的第一个参数是原始名称，
        /// 第二个参数是尝试重命名失败的次数，从2开始，以上名称均不带扩展名</param>
        /// <returns>复制后的新文件或目录</returns>
        IIO Copy(IDirectory? target, string? newName = null, Func<string, int, string>? rename = null);
        #endregion
        #region 打开
        /// <summary>
        /// 打开这个文件或目录
        /// </summary>
        /// <returns>打开文件或目录所创建的新进程</returns>
        Process Open()
           => Process.Start(Path);
        #endregion
        #region 对文件或目录执行原子操作
        #region 说明文档
        /*问：文件或目录的原子操作是什么意思？
           答：指类似数据库事务的模式，
           如果在操作过程中出现了异常，
           则不会对文件或目录产生修改
           
           问：原子操作有性能损失吗？
           答：由于可能会涉及创建备份之类的操作，
           因此对于大文件，原子操作有明显的性能损失，
           建议仅在必要时使用*/
        #endregion
        #region 有返回值
        /// <summary>
        /// 对文件或目录执行原子操作
        /// </summary>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="delegate">对文件或目录进行原子操作的委托，
        /// 参数就是这个<see cref="IIO"/>对象本身</param>
        /// <returns>执行<paramref name="delegate"/>所获取的返回值</returns>
        Ret Atomic<Ret>(Func<IIO, Ret> @delegate);
        #endregion
        #region 无返回值
        /// <summary>
        /// 对文件或目录执行原子操作
        /// </summary>
        /// <param name="delegate">对文件或目录进行原子操作的委托，
        /// 参数就是这个<see cref="IIO"/>对象本身</param>
        void Atomic(Action<IIO> @delegate)
            => Atomic<object?>(x =>
            {
                @delegate(x);
                return null;
            });
        #endregion
        #endregion
        #region 刷新文件或目录
        /// <summary>
        /// 手动刷新文件或目录的状态
        /// </summary>
        void Refresh();
        #endregion
        #endregion
    }
}
