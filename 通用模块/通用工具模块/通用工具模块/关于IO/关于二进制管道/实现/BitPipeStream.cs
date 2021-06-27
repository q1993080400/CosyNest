using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace System.IOFrancis.Bit
{
    /// <summary>
    /// 这个类型是<see cref="IBitPipe"/>的实现，
    /// 它通过流来读写二进制数据
    /// </summary>
    class BitPipeStream : IBitPipe
    {
        #region 封装的对象
        #region Stream对象
        /// <summary>
        /// 获取封装的<see cref="IO.Stream"/>对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private Stream Stream { get; }
        #endregion
        #endregion
        #region 数据的基本信息
        #region 数据的描述
        public string? Describe { get; }
        #endregion
        #region 数据的格式
        public string Format { get; }
        #endregion
        #region 数据的长度
        public long Length => Stream.Length;
        #endregion
        #endregion
        #region 操作数据
        #region 读取数据
        public async IAsyncEnumerable<byte[]> Read(long? bufferSize = null)
        {
            if (Stream.Position is not 0)
                Stream.Position = 0;
            if (bufferSize is { })
                ExceptionIntervalOut.Check(1L, null, bufferSize.Value);
            var bs = Math.Min(Length, bufferSize ?? Length);
            while (true)
            {
                var memory = new byte[bs];
                switch (await Stream.ReadAsync(memory))
                {
                    case 0:
                        yield break;
                    case var c when c < bs:
                        yield return memory[0..c]; break;
                    default:
                        yield return memory; break;
                }
            }
        }
        #endregion
        #region 写入数据
        public async Task Write(byte[] data)
        {
            await Stream.WriteAsync(data);
            await Stream.FlushAsync();
        }
        #endregion
        #region 转换为流
        public Stream ToStream()
            => new BufferedStream(Stream);
        #endregion
        #endregion
        #region 析构函数
        ~BitPipeStream()
        {
            Stream.Dispose();
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的<see cref="IO.Stream"/>对象初始化对象
        /// </summary>
        /// <param name="stream">封装的<see cref="IO.Stream"/>对象，本对象的功能就是通过它实现的</param>
        /// <param name="format">二进制数据的格式，如果格式未知，则为<see cref="string.Empty"/></param>
        /// <param name="describe">对数据的描述，如果没有描述，则为<see langword="null"/></param>
        public BitPipeStream(Stream stream, string format, string? describe)
        {
            this.Stream = stream;
            this.Format = format;
            this.Describe = describe;
        }
        #endregion
    }
}
