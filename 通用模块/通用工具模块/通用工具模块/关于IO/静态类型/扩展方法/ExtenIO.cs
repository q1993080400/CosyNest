using System.Collections.Generic;
using System.IO;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 关于IO的扩展方法
    /// </summary>
    public static class ExtenIO
    {
        #region 根据文件类型筛选文件集合
        /// <summary>
        /// 在一个文件集合中，筛选出所有与指定文件类型兼容的文件
        /// </summary>
        /// <param name="list">要筛选的集合</param>
        /// <param name="fileType">作为条件的路径对象</param>
        /// <param name="isForward">如果这个值为<see langword="true"/>，选择是这个类型的文件，
        /// 否则选择不是这个类型的文件，即取反</param>
        /// <returns></returns>
        public static IEnumerable<IFile> WhereFileType(this IEnumerable<IFile> list, IFileType fileType, bool isForward = true)
            => list.Where(x => x.IsCompatible(fileType) == isForward);
        #endregion
        #region 关于流
        #region 读取流的全部内容
        /// <summary>
        /// 读取流中的全部内容
        /// </summary>
        /// <param name="stream">待读取内容的流</param>
        /// <returns></returns>
        public static async Task<byte[]> ReadAll(this Stream stream)
        {
            var arry = new byte[stream.Length];
            await stream.ReadAsync(arry);
            return arry;
        }
        #endregion
        #endregion
        #region 将对象转换为IBitRead
        #region 转换流
        /// <summary>
        /// 将一个<see cref="Stream"/>转换为等效的<see cref="IBitPipe"/>
        /// </summary>
        /// <param name="stream">待转换的<see cref="Stream"/>对象</param>
        /// <param name="format">二进制数据的格式，如果格式未知，则为<see cref="string.Empty"/></param>
        /// <param name="describe">对数据的描述，如果没有描述，则为<see langword="null"/></param>
        /// <returns></returns>
        public static IBitPipe ToBitPipe(this Stream stream, string format = "", string? describe = null)
            => new BitPipeStream(stream, format, describe);
        #endregion
        #region 转换异步迭代器
        /// <summary>
        /// 将异步迭代器转换为<see cref="IBitRead"/>
        /// </summary>
        /// <param name="bytes">用来枚举数据的异步迭代器</param>
        /// <param name="format">二进制数据的格式，如果格式未知，则为<see cref="string.Empty"/></param>
        /// <param name="describe">对数据的描述，如果没有描述，则为<see langword="null"/></param>
        /// <returns></returns>
        public static IBitRead ToBitRead(this IAsyncEnumerable<byte[]> bytes, string format = "", string? describe = null)
            => new BitReadEnumerable(bytes, format, describe);
        #endregion
        #region 转换枚举字节数组的同步迭代器
        /// <summary>
        /// 将枚举字节数组的同步迭代器转换为<see cref="IBitRead"/>
        /// </summary>
        /// <param name="bytes">用来枚举数据的同步迭代器</param>
        /// <param name="format">二进制数据的格式，如果格式未知，则为<see cref="string.Empty"/></param>
        /// <param name="describe">对数据的描述，如果没有描述，则为<see langword="null"/></param>
        /// <returns></returns>
        public static IBitRead ToBitRead(this IEnumerable<byte[]> bytes, string format = "", string? describe = null)
            => new BitReadEnumerable(bytes.Fit(), format, describe);
        #endregion
        #region 转换枚举字节的同步迭代器
        /// <summary>
        /// 将枚举字节的同步迭代器转换为<see cref="IBitRead"/>
        /// </summary>
        /// <param name="bytes">用来枚举数据的同步迭代器</param>
        /// <param name="format">二进制数据的格式，如果格式未知，则为<see cref="string.Empty"/></param>
        /// <param name="describe">对数据的描述，如果没有描述，则为<see langword="null"/></param>
        /// <returns></returns>
        public static IBitRead ToBitRead(this IEnumerable<byte> bytes, string format = "", string? describe = null)
            => ToBitRead(new[] { bytes.ToArray() }, format, describe);
        #endregion
        #endregion
        #region 分割缓冲区
        /// <summary>
        /// 将异步迭代器中的数组的元素按照缓冲区分割
        /// </summary>
        /// <typeparam name="Obj">数组的元素类型</typeparam>
        /// <param name="datas">待分割的异步迭代器</param>
        /// <param name="bufferSize">缓冲区的大小，返回的新集合中的每个数组的元素数量最高不会超过这个参数，
        /// 如果为<see langword="null"/>，则一次返回所有元素</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Obj[]> Buffer<Obj>(this IAsyncEnumerable<Obj[]> datas, long? bufferSize = null)
        {
            if (bufferSize is null)
            {
                yield return Array.Empty<Obj>().Union(await datas.Linq(x => x.ToArray()));
                yield break;
            }
            var bs = (int)bufferSize.Value;
            await using var enumerator = datas.GetAsyncEnumerator();
            var buffer = Array.Empty<Obj>();
            while (true)
            {
                if (!await enumerator.MoveNextAsync())
                {
                    if (buffer.Any())
                        yield return buffer;
                    yield break;
                }
                buffer = buffer.Union(enumerator.Current);
                if (buffer.Length >= bs)
                {
                    yield return buffer[0..bs];
                    buffer = buffer.ElementAt(bs.., false);
                }
            }
        }
        #endregion
    }
}
