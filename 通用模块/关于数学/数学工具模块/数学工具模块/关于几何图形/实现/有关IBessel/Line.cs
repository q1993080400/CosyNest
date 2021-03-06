using System.Collections.Generic;
using System.Linq;

using static System.Maths.ToolArithmetic;

namespace System.Maths.Geometric
{
    /// <summary>
    /// 这个类型是<see cref="IBessel"/>的实现，
    /// 但是它仅能表示线段，不能表示曲线
    /// </summary>
    class Line : IBessel
    {
        #region 枚举线段的起点和终点
        public IReadOnlyList<IPoint> Node { get; }
        #endregion
        #region 返回线段的长度
        public Num Length
        {
            get
            {
                var ((br, bt), (er, et)) = this.To<IBessel>().Endpoint;
                return this.To<IBessel>().Properties switch
                {
                    BesselProperties.Horizontal => Abs(er - br),
                    BesselProperties.Vertical => Abs(et - bt),
                    BesselProperties.Slash => ToolTrigonometric.CalPythagoreanTriple(Abs(er - br), Abs(et - bt), false),
                    _ => throw new NotSupportedException("无法计算曲线的长度")
                };
            }
        }
        #endregion
        #region 返回创建线段的模型
        public IGeometricModel<IGeometric> Model { get; }
        #endregion
        #region 获取这个线段本身
        public IEnumerable<IBessel> Content
            => new[] { this };
        #endregion
        #region 获取线段的界限
        public ISizePos Boundaries
            => ToolPlane.Boundaries(Node.ToArray());
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="model">用来创建线段的模型</param>
        /// <param name="begin">线段的起点</param>
        /// <param name="end">线段的终点</param>
        public Line(IGeometricModel<IBessel> model, IPoint begin, IPoint end)
        {
            this.Model = model;
            Node = new[] { begin, end };
        }
        #endregion
    }
}
