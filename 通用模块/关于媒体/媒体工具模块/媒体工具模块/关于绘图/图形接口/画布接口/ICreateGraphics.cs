using System.MathFrancis.Plane;
using System.MathFrancis.Plane.Geometric;

namespace System.Media.Drawing.Graphics;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个图形创建者，
/// 它可以创建与指定画布兼容的基本图形
/// </summary>
public interface ICreateGraphics
{
    #region 创建图形
    #region 说明文档
    /*实现这些API请遵循以下规范：
      #在创建新图形后，自动带有默认样式，
      也就是它们的Style属性不能为null，
      这样可以为设置样式提供方便

      #在执行创建图形的方法后，
      自动将图形添加到画布中*/
    #endregion
    #region 创建文本
    /// <summary>
    /// 创建文本，并返回
    /// </summary>
    /// <param name="text">画布文本所包含的文本</param>
    /// <param name="point">文本的位置，如果为<see langword="null"/>，默认为(0,0)</param>
    /// <returns></returns>
    IGraphicsText CreateText(string text, IPoint? point = null);
    #endregion
    #region 创建几何图形
    /// <summary>
    /// 在画布上创建一个几何图形，并返回
    /// </summary>
    /// <typeparam name="Geometric">要创建的几何图形的类型</typeparam>
    /// <param name="model">用于创建几何图形的模型</param>
    /// <param name="point">几何图形的位置，如果为<see langword="null"/>，默认为(0,0)</param>
    /// <returns></returns>
    IGraphicsGeometric<Geometric> CreateGeometric<Geometric>(IGeometricModel<Geometric> model, IPoint? point = null)
        where Geometric : IGeometric;
    #endregion
    #endregion
    #region 创建样式
    /// <summary>
    /// 创建一个样式对象，并返回
    /// </summary>
    /// <typeparam name="Style">样式的类型</typeparam>
    /// <returns></returns>
    Style CreateStyle<Style>()
        where Style : IGraphicsStyle;
    #endregion
}
