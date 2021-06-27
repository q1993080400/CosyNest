using System.Collections.Generic;

namespace System.Maths.Geometric
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个几何图形
    /// </summary>
    public interface IGeometric
    {
        #region 返回几何模型
        /// <summary>
        /// 获取创建该几何图形的几何模型
        /// </summary>
        IGeometricModel<IGeometric> Model { get; }
        #endregion
        #region 获取组成几何图形的线段
        /// <summary>
        /// 获取组成该几何图形的线段或曲线，
        /// 这个迭代器中的每个元素代表一个绘制阶段
        /// </summary>
        IEnumerable<IBessel> Content { get; }

        /*问：什么是绘制阶段？
          答：绘制阶段指的是在绘制几何图形时，
          依次绘制的每一条线段或贝塞尔曲线，
          举例说明，一个矩形有四个绘制阶段，
          分别是它的上边，右边，底边和左边
        
          问：为什么不将这个属性的类型设计为IReadOnlyList<IBessel>？
          这样可以更方便地实现一些操作，例如获取矩形的第一和第三条边
          答：因为这样的话，这个操作会依赖于绘制图形的顺序，
          作者认为这样很容易产生潜在Bug，因为不能保证每个人绘制几何图形的习惯都是相同的*/
        #endregion
        #region 获取几何图形的界限
        /// <summary>
        /// 获取几何图形的界限，
        /// 它代表容纳该几何图形，
        /// 所需要的最小平面的位置和大小
        /// </summary>
        ISizePos Boundaries { get; }
        #endregion
    }
}
