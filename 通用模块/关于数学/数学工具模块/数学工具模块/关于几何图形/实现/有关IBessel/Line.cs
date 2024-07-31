
using static System.MathFrancis.ToolArithmetic;

namespace System.MathFrancis.Plane.Geometric;

/// <summary>
/// 这个类型是<see cref="IBessel"/>的实现，
/// 但是它仅能表示线段，不能表示曲线
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="model">用来创建线段的模型</param>
/// <param name="begin">线段的起点</param>
/// <param name="end">线段的终点</param>
sealed class Line(IGeometricModel<IBessel> model, IPoint begin, IPoint end) : IBessel
{
    #region 枚举线段的起点和终点
    public IReadOnlyList<IPoint> Node { get; } = [begin, end];
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
    public IGeometricModel<IGeometric> Model { get; } = model;
    #endregion
    #region 获取这个线段本身
    public IEnumerable<IBessel> Content
        => [this];
    #endregion
    #region 获取线段的界限
    public ISizePos Boundaries
        => ToolPlane.Boundaries(Node.ToArray());

    #endregion
}
