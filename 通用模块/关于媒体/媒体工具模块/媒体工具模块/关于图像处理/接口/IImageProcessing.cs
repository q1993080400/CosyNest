using System.Maths.Plane;

namespace System.Media.Drawing;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以执行图像处理任务
/// </summary>
public interface IImageProcessing
{
    #region 格式转换
    /// <summary>
    /// 转换图像的格式
    /// </summary>
    /// <param name="imagePath">要转换的图像的路径</param>
    /// <param name="targetPath">保存转换后图像的目标路径</param>
    /// <param name="maxSize">新图像的最大大小，以像素为单位，
    /// 它的横纵比并不重要，以总像素数量为准</param>
    /// <param name="cancellation">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task FormatConversion(string imagePath, string targetPath, ISizePixel? maxSize = null, CancellationToken cancellation = default);
    #endregion
}
