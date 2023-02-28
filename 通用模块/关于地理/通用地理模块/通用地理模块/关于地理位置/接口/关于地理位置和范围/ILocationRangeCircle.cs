using System.Maths;

namespace System.Geography;

/// <summary>
/// 这个类型是一个拥有圆形范围的地理位置
/// </summary>
public interface ILocationRangeCircle : ILocationRange
{
    #region 半径
    /// <summary>
    /// 获取圆形半径的长度
    /// </summary>
    IUnit<IUTLength> Radius { get; }
    #endregion
}
