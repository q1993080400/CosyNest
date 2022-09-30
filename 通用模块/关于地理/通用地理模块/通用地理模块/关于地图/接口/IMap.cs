using System.Maths;

namespace System.Geography.Map;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个地图
/// </summary>
public interface IMap
{
    #region 返回地理信息
    /// <summary>
    /// 返回某一地点周围的地理位置信息
    /// </summary>
    /// <typeparam name="Info">返回的地理位置信息的类型</typeparam>
    /// <param name="location">地点位置，
    /// 如果为<see langword="null"/>，则定位到附近位置，如果不能定位，则引发异常</param>
    /// <param name="radius">指定以<paramref name="location"/>为圆心，搜索地理位置的半径，
    /// 如果为<see langword="null"/>，则默认为1000米</param>
    /// <returns></returns>
    IAsyncEnumerable<Info> GetGeographicInfo<Info>(ILocation? location = null, IUnit<IUTLength>? radius = null)
        where Info : IGeographicInfo;
    #endregion
}
