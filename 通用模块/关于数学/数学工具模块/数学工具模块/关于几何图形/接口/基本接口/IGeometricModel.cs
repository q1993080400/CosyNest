namespace System.Maths.Plane.Geometric;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个几何模型，
/// 它是用于创建几何图形的工厂
/// </summary>
/// <typeparam name="Geometric">几何模型所创建的几何图形的类型</typeparam>
public interface IGeometricModel<out Geometric>
    where Geometric : IGeometric
{
    #region 说明文档
    /*问：什么是几何模型？它和几何图形有什么区别？
      答：几何模型的抽象程度更高，
      它仅仅规定了如何使用线段和曲线组成几何图形，
      而不考虑该几何图形的位置，以及是否被旋转，
      而几何图形已经被描绘在一个固定的位置，
      按照编程的术语来说，几何模型是创建几何图形的工厂，
      而几何图形是工厂的产品

      问：为什么要区分几何模型和几何图形？
      答：这个设计有数学和编程两方面的考虑，
      从数学上来说，几何图形的性质与它的绘制方法无关，
      举例说明，任何直径相等的圆，无论它们处于什么地方，被描绘在什么载体上，都应该被视为相等，
      因此有必要将抽象的几何模型和具象的几何图形区分开来，
      从编程的角度来说，这种设计可以更方便的在任何地方绘制几何模型，
      或者将已经存在的几何图形复制到其他地方*/
    #endregion
    #region 绘制几何图形
    /// <summary>
    /// 将这个几何图形绘制出来，
    /// 并返回绘制好的几何图形
    /// </summary>
    /// <param name="position">指定几何图形左上角的坐标，
    /// 注意：对于非矩形，这个坐标指的是容纳该几何图形，所需要的最小平面的左上角坐标，
    /// 如果为<see langword="null"/>，则为(0,0)</param>
    /// <returns></returns>
    Geometric Draw(IPoint? position = null);
    #endregion
}
