using System.IOFrancis;
using System.MathFrancis.Plane;
using System.Media.Drawing.Graphics;

namespace System.Media.Drawing;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个图像
/// </summary>
public interface IImage : IFromIO
{
    #region 说明文档
    /*实现本接口请遵循以下规范：
      在保存图片时，如果在路径参数指定了不同的文件名，
      则自动转换格式，前提是底层对象支持这个操作*/
    #endregion
    #region 获取图片的大小
    /// <summary>
    /// 获取图片的大小
    /// </summary>
    ISize Size { get; }

    /*问：在本项目的旧版本中，这个属性是位图专有的，
      为什么现在要把它扩展到所有图片？
      答：作者原本认为矢量图没有图片大小的概念，因此不需要这个属性，
      但是作者现在认识到，矢量图只是不会因为缩放而失真，
      并不代表不存在缩放这个概念，因此就把它加了回来

      问：图片的高度和宽度使用什么单位？
      答：对于位图，单位是像素，对于矢量图，单位是数学坐标*/
    #endregion
    #region 返回图像的细节
    /// <summary>
    /// 返回图像的细节，
    /// 也就是组成图像的所有图形
    /// </summary>
    IEnumerable<IGraphics> Details { get; }

    /*说明文档：
      如果这个图像是位图，
      则这个集合的元素的唯一合法类型是IGraphicsPixel*/
    #endregion
}
