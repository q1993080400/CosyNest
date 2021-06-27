using System.Collections.Generic;
using System.DrawingFrancis.Graphics;
using System.IO;
using System.IOFrancis.Bit;
using System.Maths;

namespace System.DrawingFrancis
{
    /// <summary>
    /// 这个类型代表一个存在于内存中的图像
    /// </summary>
    class ImageMemory : IImage
    {
        #region 关于图像的内容
        #region 返回图像的二进制表示方式
        /// <summary>
        /// 返回图像的二进制表示方式
        /// </summary>
        private ReadOnlyMemory<byte> ImageBinary { get; }
        #endregion
        #region 获取或设置图片的大小
        public ISize Size
        {
            get => throw new NotSupportedException("本类型仅提供从内存加载图片的服务，不支持读取图片的大小");
            set => throw new NotSupportedException("本类型仅提供从内存加载图片的服务，不支持写入图片的大小");
        }
        #endregion
        #region 返回图像的格式
        public string Format { get; }
        #endregion
        #region 读取图像
        public IBitRead Read()
            => new MemoryStream(ImageBinary.ToArray(), false).ToBitPipe(Format);
        #endregion
        #region 枚举图像的细节
        public IEnumerable<IGraphics> Details
            => throw new NotSupportedException("本类型只提供从内存加载图像的服务，不支持枚举图像的细节");
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的字节数组和格式初始化对象
        /// </summary>
        /// <param name="imageBinary">图片的二进制形式</param>
        /// <param name="format">图片的格式</param>
        public ImageMemory(ReadOnlyMemory<byte> imageBinary, string format)
        {
            if (format.IsVoid())
                throw new ArgumentException("图片的格式不能为null或空字符串");
            this.ImageBinary = imageBinary;
            this.Format = format;
        }
        #endregion
    }
}
