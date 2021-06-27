using System.DrawingFrancis.Graphics;
using System.DrawingFrancis.Text;
using System.IO;
using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Linq;
using System.Maths;
using System.Threading.Tasks;

namespace System.DrawingFrancis
{
    /// <summary>
    /// 这个静态类可以帮助创建一些关于绘图的对象
    /// </summary>
    public static class CreateDrawingObj
    {
        #region 创建文本格式
        /// <summary>
        /// 用指定的参数创建文本样式
        /// </summary>
        /// <param name="fontName">指定的字体</param>
        /// <param name="size">字体的大小</param>
        /// <param name="color">文本的颜色</param>
        public static ITextStyleVar TextStyleVar(string fontName, IUnit<IUTFontSize> size, IColor color)
            => new TextStyleVar(fontName, size, color);
        #endregion
        #region 创建IColor
        #region 指定RGBA
        /// <summary>
        /// 使用指定的红色，绿色，蓝色和透明度创建<see cref="IColor"/>
        /// </summary>
        /// <param name="r">指定的红色值</param>
        /// <param name="g">指定的绿色值</param>
        /// <param name="b">指定的蓝色值</param>
        /// <param name="a">指定的透明度</param>
        public static IColor Color(byte r, byte g, byte b, byte a = 255)
            => new Color(r, g, b, a);
        #endregion
        #region 指定16进制字符串
        /// <summary>
        /// 根据一个用16进制表示的字符串，创建一个<see cref="IColor"/>并返回
        /// </summary>
        /// <param name="sys16">指示ARGB值的十六进制字符串，格式为FF001122</param>
        /// <returns></returns>
        public static IColor Color(string sys16)
        {
            var array = Convert.FromHexString(sys16);
            if (array.Length is not 4)
                throw new ArgumentException($"{nameof(sys16)}不是合法的表示颜色的字符串");
            return Color(array[0], array[1], array[2], array[3]);
        }
        #endregion
        #region 生成随机颜色
        /// <summary>
        /// 生成透明度指定，但RGB随机的颜色
        /// </summary>
        /// <param name="alpha">颜色的透明度</param>
        /// <param name="rand">用来生成随机数的对象，如果为<see langword="null"/>，则使用一个默认对象</param>
        /// <returns></returns>
        public static IColor ColorRandom(byte alpha = 255, IRandom? rand = null)
        {
            rand ??= CreateBaseMath.RandomOnly;
            byte Fun()
                => (byte)rand.RandRange(0, 255);
            return Color(Fun(), Fun(), Fun(), alpha);
        }
        #endregion
        #endregion 
        #region 创建像素点
        /// <summary>
        /// 使用指定的颜色，位置和图层创建像素点
        /// </summary>
        /// <param name="color">像素点的颜色</param>
        /// <param name="position">像素点的位置</param>
        /// <param name="layer">像素点的图层</param>
        public static IGraphicsPixel GraphicsPixel(IColor color, IPoint position, int layer = 0)
            => new GraphicsPixel(color, position, layer);
        #endregion
        #region 创建内存图像
        #region 指定二进制内容和格式
        /// <summary>
        /// 使用指定的二进制内容和格式创建一个存在于内存中的图像
        /// </summary>
        /// <param name="imageBinary">图片的二进制形式</param>
        /// <param name="format">图片的格式</param>
        /// <returns></returns>
        public static IImage ImageMemory(ReadOnlyMemory<byte> imageBinary, string format)
            => new ImageMemory(imageBinary, format);
        #endregion
        #region 指定流和格式
        /// <summary>
        /// 从指定的流中读取字节数组，并创建具有指定格式的内存图像
        /// </summary>
        /// <param name="stream">用来读取图像内容的流</param>
        /// <param name="format">图片的格式</param>
        /// <returns></returns>
        public static async Task<IImage> ImageMemory(Stream stream, string format)
        {
            var arry = await stream.ReadAll();
            return ImageMemory(arry, format);
        }
        #endregion
        #region 指定文件和格式
        /// <summary>
        /// 将文件读取到内存，并返回读取到的图片
        /// </summary>
        /// <param name="path">图片所在的文件</param>
        /// <returns></returns>
        public static async Task<IImage> ImageMemory(PathText path)
        {
            var stream = CreateIO.File(path).GetBitPipe();
            return ImageMemory(await stream.Read().Linq(x => x.First()), ToolPath.SplitPathFile(path).Extended);
        }
        #endregion
        #endregion
    }
}
