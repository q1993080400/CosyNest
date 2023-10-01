using System.MathFrancis;

namespace System.Geography.Map;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来表示一个地理位置信息，
/// 它表示地图上的某一点
/// </summary>
public interface IGeographicInfo
{
    #region 地理位置
    /// <summary>
    /// 获取这个地理信息所在的位置
    /// </summary>
    ILocation Location { get; }
    #endregion
    #region 距离
    /// <summary>
    /// 如果程序知道用户的位置，
    /// 则返回该地点与用户的距离，
    /// 否则为<see langword="null"/>
    /// </summary>
    IUnit<IUTLength>? Distance { get; }
    #endregion
    #region 名称
    /// <summary>
    /// 获取该地理位置信息的名称
    /// </summary>
    string Name { get; }
    #endregion
}
