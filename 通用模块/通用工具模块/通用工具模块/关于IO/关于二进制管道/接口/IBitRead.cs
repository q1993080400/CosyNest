using System.Collections.Generic;
using System.IO;
using System.IOFrancis.FileSystem;
using System.Linq;
using System.Threading.Tasks;

namespace System.IOFrancis.Bit
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以直接读取二进制数据
    /// </summary>
    public interface IBitRead : IBitPipeBase
    {
        #region 说明文档
        /*问：新版Net推荐使用值类型（例如Memory）来代替字节数组，
          请问为什么本接口不这样做？
          答：因为为了方便考虑，Read方法支持一次读取所有数据，
          如果返回值类型的话，在数据较多时复制该对象的成本会非常可怕*/
        #endregion
        #region 将数据保存到文件中
        /// <summary>
        /// 将数据保存到文件中
        /// </summary>
        /// <param name="path">要保存的路径，
        /// 如果<see cref="IBitPipeBase.Format"/>不为<see langword="null"/>，不要加上扩展名</param>
        /// <param name="bufferSize">指定缓冲区的字节数量，
        /// 如果为<see langword="null"/>，则一次写入全部数据</param>
        /// <returns></returns>
        async Task SaveToFile(PathText path, long? bufferSize = null)
        {
            using var stream = new FileStream($"{(Format.IsVoid() ? path : $"{path}.{Format}")}", FileMode.Create);
            await foreach (var item in Read(bufferSize))
            {
                await stream.WriteAsync(item);
            }
        }
        #endregion
        #region 读取二进制流
        /// <summary>
        /// 以异步流的形式读取二进制数据
        /// </summary>
        /// <param name="bufferSize">指定缓冲区的字节数量，
        /// 如果为<see langword="null"/>，则一次读取全部数据</param>
        /// <returns></returns>
        IAsyncEnumerable<byte[]> Read(long? bufferSize = null);

        /*实现本API请遵循以下规范：
          #每次遍历该迭代器时，都应该从数据的开头返回数据，
          换言之，在多次读取数据时，不应该像Stream一样需要手动的复位操作
        
          #如果bufferSize为null或Length，
          则Read方法应该只返回一个字节数组，
          也就是该数据的全部内容
        
          #本API应该是一个纯函数，换言之，
          修改返回的字节数组中的值，不应该影响下一次调用方法返回的数据，
          由于本API需要分割缓冲区，因此这个规范很容易满足*/
        #endregion
        #region 读取二进制流（不拆分缓冲区）
        /// <summary>
        /// 一次性读取全部二进制数据，且不拆分缓冲区，
        /// 如果没有任何数据，则返回一个空数组
        /// </summary>
        /// <returns></returns>
        async Task<byte[]> ReadComplete()
             => (await Read().Linq(x => x.FirstOrDefault())) ?? Array.Empty<byte>();
        #endregion
        #region 复制二进制数据
        /// <summary>
        /// 读取这个管道的二进制数据，
        /// 并将其复制到另一个管道中
        /// </summary>
        /// <param name="write">复制的目标管道</param>
        /// <param name="bufferSize">指定缓冲区的字节数量，
        /// 如果为<see langword="null"/>，则一次复制全部数据</param>
        /// <returns></returns>
        async Task CopyTo(IBitWrite write, long? bufferSize = null)
        {
            await foreach (var item in Read(bufferSize))
            {
                await write.Write(item);
            }
        }
        #endregion
    }
}
