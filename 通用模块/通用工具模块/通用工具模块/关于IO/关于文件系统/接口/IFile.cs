using System.Collections.Generic;
using System.IO;
using System.IOFrancis.Bit;
using System.Linq;

namespace System.IOFrancis.FileSystem
{
    /// <summary>
    /// 所有实现这个接口的类型，都可以视为一个文件
    /// </summary>
    public interface IFile : IIO
    {
        #region 文件的信息
        #region 读写文件的简称
        /// <summary>
        /// 获取或设置文件的简单名称，
        /// 它不带扩展名
        /// </summary>
        string NameSimple
        {
            get => IO.Path.GetFileNameWithoutExtension(Path);
            set => Path = ToolPath.RefactoringPath(Path,
                value ?? throw new ArgumentNullException($"{NameSimple}禁止写入null值"), null);
        }
        #endregion
        #region 读写扩展名
        /// <summary>
        /// 当读取这个属性的时候，获取文件的扩展名，不带点号，
        /// 当写入这个属性的时候，修改文件的扩展名，
        /// 如果写入或返回<see cref="string.Empty"/>，代表没有扩展名
        /// </summary>
        string NameExtension
        {
            get => ToolPath.SplitPathFile(Path, false).Extended;
            set => Path = ToolPath.RefactoringPath(Path, null,
                value ?? throw new ArgumentNullException($"{NameExtension}禁止写入null值"));
        }
        #endregion
        #region 关于文件类型
        #region 返回文件类型
        /// <summary>
        /// 返回这个文件已注册的文件类型
        /// </summary>
        IEnumerable<IFileType> FileTypes
            => NameExtension switch
            {
                null => Array.Empty<IFileType>(),
                var n => IFileType.RegisteredFileType.
                TryGetValue(n).Value ?? Array.Empty<IFileType>()
            };
        #endregion
        #region 判断文件类型是否兼容
        /// <summary>
        /// 检查这个文件是否与一个文件类型兼容
        /// </summary>
        /// <param name="fileType">要判断的文件类型</param>
        /// <returns>如果兼容，返回<see langword="true"/>，否则返回<see langword="false"/></returns>
        bool IsCompatible(IFileType fileType)
           => fileType.IsCompatible(Path);
        #endregion
        #endregion
        #endregion
        #region 对文件的操作
        #region 复制文件
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="target">复制的目标目录，
        /// 如果为<see langword="null"/>，则默认复制到当前目录</param>
        /// <param name="newSimple">新文件的名称，不带扩展名，如果为<see langword="null"/>，代表不修改</param>
        /// <param name="newExtension">新文件的扩展名，如果为<see langword="null"/>，代表不修改</param>
        ///  <param name="rename">当复制的目标目录存在同名文件时，
        /// 如果这个值为<see langword="null"/>，则覆盖同名文件，
        /// 否则执行该委托赋予一个新的名称，委托的第一个参数是不带扩展名的原始名称，
        /// 第二个参数是尝试重命名失败的次数，从2开始，返回值就是重命名后的新名称，以上名称均不带扩展名</param>
        /// <returns>复制后的新文件</returns>
        IFile Copy(IDirectory? target, string? newSimple, string? newExtension, Func<string, int, string>? rename = null)
            => (IFile)Copy(target,
                ToolPath.GetFullName(newSimple ?? NameSimple, newExtension ?? NameExtension), rename);
        #endregion
        #endregion
        #region 创建数据管道
        /// <summary>
        /// 创建一个可以读写文件的数据管道
        /// </summary>
        /// <param name="mod">指定打开文件的方式</param>
        /// <returns></returns>
        IBitPipe GetBitPipe(FileMode mod = FileMode.Open)
            => new FileStream(Path, mod).ToBitPipe(NameExtension, NameSimple);
        #endregion
    }
}
