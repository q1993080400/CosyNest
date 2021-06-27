using System.Maths;

namespace System.DrawingFrancis
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个位图
    /// </summary>
    public interface IBitmap : IImage
    {
        #region 说明文档
        /*实现本接口请遵循以下规范：
          #本接口实现了ISize，
          平面的长度和宽度以像素为单位*/
        #endregion
        #region 获取或设置的大小
        /// <summary>
        /// 获取或设置图片的大小
        /// </summary>
        /// <exception cref="ArgumentNullException">该属性写入<see langword="null"/></exception>
        new ISizePixel Size { get; set; }
        #endregion
        #region 获取图像的每个像素点
        /// <summary>
        /// 获取图像的每个像素点
        /// </summary>
        /// <returns></returns>
        IColor[,] Pixel();
        #endregion
    }
}
