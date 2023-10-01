namespace System.MathFrancis.Plane.Geometric;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一条贝塞尔曲线，
/// 如果仅有两个控制点，还可以表示一条线段
/// </summary>
public interface IBessel : IGeometric
{
    #region 获取节点
    /// <summary>
    /// 枚举贝塞尔曲线的所有控制点
    /// </summary>
    IReadOnlyList<IPoint> Node { get; }
    #endregion
    #region 获取端点
    /// <summary>
    /// 获取贝塞尔曲线的端点，
    /// 也就是它的起点和终点坐标
    /// </summary>
    (IPoint Begin, IPoint End) Endpoint
        => (Node[0], Node[^1]);
    #endregion
    #region 获取长度
    /// <summary>
    /// 获取该贝塞尔曲线的长度
    /// </summary>
    Num Length { get; }

    /*实现本API请遵循以下规范：
      对于线段，这个属性应返回线段的长度，
      对于曲线，应返回将曲线拉直后的长度*/
    #endregion
    #region 指示贝塞尔曲线的性质
    /// <summary>
    /// 指示这条贝塞尔曲线的性质
    /// </summary>
    BesselProperties Properties
    {
        get
        {
            if (Node.Count is not 2)
                return BesselProperties.Curve;
            var ((br, bt), (er, et)) = Endpoint;
            if (br == er)
                return BesselProperties.Vertical;
            return bt == et ? BesselProperties.Horizontal : BesselProperties.Slash;
        }
    }
    #endregion
}
#region 指示贝塞尔曲线的性质
/// <summary>
/// 该枚举指示一条贝塞尔曲线的性质
/// </summary>
public enum BesselProperties
{
    /// <summary>
    /// 表示该贝塞尔曲线实际是一条水平线段
    /// </summary>
    Horizontal,
    /// <summary>
    /// 表示该贝塞尔曲线实际是一条垂直线段
    /// </summary>
    Vertical,
    /// <summary>
    /// 表示该贝塞尔曲线实际是一条斜线线段
    /// </summary>
    Slash,
    /// <summary>
    /// 表示该贝塞尔曲线是一条真正的曲线
    /// </summary>
    Curve
}
#endregion