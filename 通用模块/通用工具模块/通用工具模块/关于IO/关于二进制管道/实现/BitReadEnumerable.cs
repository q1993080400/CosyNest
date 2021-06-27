using System.Collections.Generic;
using System.IO;

namespace System.IOFrancis.Bit
{
    /// <summary>
    /// 这个类型是<see cref="IBitRead"/>的实现，
    /// 它可以通过<see cref="IAsyncEnumerable{T}"/>来读取二进制数据
    /// </summary>
    class BitReadEnumerable : IBitRead
    {
        #region 封装的异步迭代器
        /// <summary>
        /// 获取封装的异步迭代器对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private IAsyncEnumerable<byte[]> Bytes { get; }
        #endregion
        #region 管道的信息
        #region 数据格式
        public string Format { get; }
        #endregion
        #region 对数据的描述
        public string? Describe { get; }
        #endregion
        #region 二进制数据的总长度
        public long Length
            => throw CreateException.NotSupported();
        #endregion
        #endregion
        #region 转换为流
        public Stream ToStream()
            => CreateIO.StreamEnumerable(Bytes);
        #endregion
        #region 读取数据
        public IAsyncEnumerable<byte[]> Read(long? bufferSize = null)
            => Bytes.Buffer(bufferSize);
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="bytes">指定的异步迭代器对象，本对象的功能就是通过它实现的</param>
        /// <param name="format">二进制数据的格式，如果格式未知，则为<see cref="string.Empty"/></param>
        /// <param name="describe">对数据的描述，如果没有描述，则为<see langword="null"/></param>
        public BitReadEnumerable(IAsyncEnumerable<byte[]> bytes, string format, string? describe)
        {
            this.Bytes = bytes;
            this.Format = format;
            this.Describe = describe;
        }
        #endregion
    }
}
