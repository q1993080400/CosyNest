using System.Maths;

namespace System.Geography;

/// <summary>
/// 这个类型是<see cref="ILocationRangeCircle"/>的实现，
/// 可以视为一个拥有圆形半径的地理位置
/// </summary>
sealed class LocationRangeCircle : ILocationRangeCircle
{
    #region 公开成员
    #region 圆形半径
    public required IUnit<IUTLength> Radius { get; init; }
    #endregion
    #region 大致位置
    public required ILocation Position { get; init; }
    #endregion
    #region 检查位置是否位于范围内
    public bool InRange(ILocation location)
        => Position.Ranging(location) <= Radius;
    #endregion
    #endregion 
}
