using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System.IOFrancis.Bit
{
    /// <summary>
    /// 这个流可以借助<see cref="IEnumerator{T}"/>来获取二进制数据
    /// </summary>
    class EnumerableStream : Stream
    {
        #region 封装的对象
        #region 枚举器
        /// <summary>
        /// 获取一个枚举所有数据的异步枚举器，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private IEnumerator<byte[]> Bytes { get; }
        #endregion
        #region 剩余数据
        /// <summary>
        /// 获取或设置剩余数据，
        /// 它是上次读取字节数组后，
        /// 由于缓冲区的限制没有返回的部分
        /// </summary>
        private byte[] OldData { get; set; } = Array.Empty<byte>();
        #endregion
        #endregion
        #region 不支持的成员
        #region 返回流的长度
        public override long Length
            => throw CreateException.NotSupported();
        #endregion
        #region 清除缓冲区
        public override void Flush()
        {

        }
        #endregion
        #region 写入流
        public override void Write(byte[] buffer, int offset, int count)
            => throw CreateException.NotSupported();
        #endregion
        #region 设置流的长度
        public override void SetLength(long value)
            => throw CreateException.NotSupported();
        #endregion
        #endregion
        #region 有关位置
        #region 查找流
        public override long Seek(long offset, SeekOrigin origin)
        {
            if ((offset, origin) is not (0, SeekOrigin.Begin))
                throw new NotSupportedException($"仅支持跳转到流的开头，" +
                    $"除{nameof(offset)}为0，{nameof(origin)}为{SeekOrigin.Begin}以外，" +
                    $"传入其他任何参数都将引发异常");
            return Position = 0;
        }
        #endregion
        #region 流的位置
        private long PositionField;

        public override long Position
        {
            get => PositionField;
            set
            {
                if (value != 0)
                    throw new NotSupportedException("仅支持跳转到流的开头，所以该属性只能写入0");
                Bytes.Reset();
                PositionField = 0;
            }
        }
        #endregion
        #endregion
        #region 指示流支持的功能
        #region 是否可读取
        public override bool CanRead => true;
        #endregion
        #region 是否可写入
        public override bool CanWrite => false;
        #endregion
        #region 是否可跳转
        public override bool CanSeek => false;
        #endregion
        #endregion
        #region 读取数据
        public override int Read(byte[] buffer, int offset, int count)
        {
            #region 本地函数
            byte[] Fun(byte[] old)
            {
                #region 说明文档
                /*本函数会递归合并数组，
                  以确保最终获取的数组元素数量不低于count，
                  或遍历至Bytes的末尾*/
                #endregion
                if (old.Length >= count || !Bytes.MoveNext())
                    return old;
                var sum = old.Union(Bytes.Current);
                return Fun(sum);
            }
            #endregion
            var arry = Fun(OldData);
            count = Math.Min(arry.Length, count);
            Array.Copy(arry, 0, buffer, offset, count);
            PositionField += count;
            OldData = arry.ElementAt(count.., false);
            return count;
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="bytes">一个枚举所有数据的枚举器，本对象的功能就是通过它实现的</param>
        public EnumerableStream(IEnumerator<byte[]> bytes)
        {
            this.Bytes = bytes;
        }
        #endregion
    }
}
