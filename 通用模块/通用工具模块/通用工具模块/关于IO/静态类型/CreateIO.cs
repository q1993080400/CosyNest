using System.Collections.Generic;
using System.IO;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;
using System.Linq;
using System.Text;

namespace System.IOFrancis
{
    /// <summary>
    /// 这个静态类可以用来创建和IO有关的对象
    /// </summary>
    public static class CreateIO
    {
        #region 返回当前文件系统
        /// <summary>
        /// 返回当前文件系统，
        /// 它是所有文件，目录，驱动器的根
        /// </summary>
        public static IFileSystem FileSystem { get; }
        = new FileSystemRealize();
        #endregion
        #region 创建文件或目录对象
        #region 创建一个文件对象
        /// <summary>
        /// 用指定的路径初始化文件对象，
        /// 不允许指定不存在的路径
        /// </summary>
        /// <param name="path">指定的路径</param>
        /// <param name="checkExist">在文件不存在的时候，如果这个值为<see langword="true"/>，
        /// 则抛出一个异常，为<see langword="false"/>，则不会抛出异常，而是会创建一个新文件</param>
        public static IFile File(PathText path, bool checkExist = true)
            => new FileRealize(path, checkExist);
        #endregion
        #region 创建一个目录对象
        /// <summary>
        /// 用指定的路径初始化目录对象
        /// </summary>
        /// <param name="path">指定的路径</param>
        /// <param name="checkExist">在路径不存在的时候，如果这个值为<see langword="true"/>，会抛出一个异常，
        /// 如果为<see langword="false"/>，则不会抛出异常，而是会创建这个目录</param>
        public static IDirectory Directory(PathText path, bool checkExist = true)
            => new DirectoryRealize(path, checkExist);
        #endregion
        #region 根据路径，返回IO对象
        /// <summary>
        /// 如果一个路径是文件，返回文件对象，
        /// 是目录，返回目录对象，
        /// 不存在，返回<see langword="null"/>
        /// </summary>
        /// <param name="path">要检查的路径</param>
        /// <returns></returns>
        public static IIO? IO(PathText path)
            => ToolPath.GetPathState(path) switch
            {
                PathState.ExistDirectory => new DirectoryRealize(path),
                PathState.ExistFile => new FileRealize(path),
                _ => null
            };
        #endregion
        #endregion
        #region 创建委托
        #region 有关读写字符串
        #region 读取
        /// <summary>
        /// 创建一个<see cref="ObjRead{Obj}"/>，
        /// 它能够通过二进制管道读取文本，文本按行返回
        /// </summary>
        /// <param name="encoder">文本的编码，
        /// 如果为<see langword="null"/>，则自动获取</param>
        /// <returns></returns>
        public static ObjRead<string> ObjReadString(Encoding? encoder = null)
            => read =>
            {
                #region 本地函数
                static async IAsyncEnumerable<string> Fun(Encoding? encoder, IBitRead read)
                {
                    using var stream = read.ToStream();
                    using StreamReader streamReader = encoder is null ? new(stream) : new(stream, encoder);
                    while (true)
                    {
                        var text = await streamReader.ReadLineAsync();
                        if (text is null)
                            yield break;
                        yield return text;
                    }
                }
                #endregion
                return Fun(encoder, read);
            };
        #endregion
        #region 写入（直接写入）
        /// <summary>
        /// 创建一个<see cref="ObjWrite{Obj}"/>，
        /// 它能够向二进制管道写入文本
        /// </summary>
        /// <param name="encoder">文本的编码，
        /// 如果为<see langword="null"/>，则为UTF8</param>
        /// <returns></returns>
        public static ObjWrite<string> ObjWriteString(Encoding? encoder = null)
        {
            encoder ??= Encoding.UTF8;
            return async (write, text) =>
            {
                var bytes = encoder.GetBytes(text);
                await write.Write(bytes);
            };
        }
        #endregion
        #region 写入（按行写入）
        /// <summary>
        /// 创建一个<see cref="ObjWrite{Obj}"/>，
        /// 它能够向二进制管道按行写入文本
        /// </summary>
        /// <param name="encoder">文本的编码，
        /// 如果为<see langword="null"/>，则为UTF8</param>
        /// <returns></returns>
        public static ObjWrite<string> ObjWriteStringLine(Encoding? encoder = null)
        {
            var write = ObjWriteString(encoder);
            return (w, t) => write(w, t + Environment.NewLine);
        }
        #endregion
        #endregion
        #endregion
        #region 创建流
        #region 通过迭代器枚举数据
        #region 同步迭代器
        /// <summary>
        /// 创建一个<see cref="Stream"/>，
        /// 它通过迭代器获取二进制数据
        /// </summary>
        /// <param name="datas">用来获取数据的迭代器</param>
        /// <returns></returns>
        public static Stream StreamEnumerable(IEnumerable<byte[]> datas)
            => new EnumerableStream(datas.GetEnumerator());
        #endregion
        #region 异步迭代器
        /// <summary>
        /// 创建一个<see cref="Stream"/>，
        /// 它通过异步流获取二进制数据
        /// </summary>
        /// <param name="datas">用来获取数据的异步流</param>
        /// <returns></returns>
        public static Stream StreamEnumerable(IAsyncEnumerable<byte[]> datas)
            => new EnumerableStream(datas.Fit().GetEnumerator());
        #endregion
        #endregion
        #endregion
        #region 创建IFileType
        #region 传入文件名
        /// <summary>
        /// 用指定的扩展名集合创建文件类型，
        /// 注意：执行此方法会将文件类型注册
        /// </summary>
        /// <param name="description">对文件类型的说明</param>
        /// <param name="extension">指定的扩展名集合，不带点号</param>
        public static IFileType FileType(string description, params string[] extension)
            => new FileType(description, extension);
        #endregion
        #region 传入文件类型
        /// <summary>
        /// 用指定的文件类型创建文件类型，
        /// 注意：执行此方法会将文件类型注册
        /// </summary>
        /// <param name="description">对文件类型的说明</param>
        /// <param name="compatible">初始化后的新对象将会兼容以上所有文件类型</param>
        public static IFileType FileType(string description, params IFileType[] compatible)
            => FileType(description,
                 compatible.Select(x => x.ExtensionName).UnionNesting(true).ToArray());
        #endregion
        #endregion
    }
}
