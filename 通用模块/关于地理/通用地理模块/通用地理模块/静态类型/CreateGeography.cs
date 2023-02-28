using System.Geography.Weather;
using System.Maths;

namespace System.Geography;

/// <summary>
/// 这个静态类可以用来创建和地理有关的对象
/// </summary>
public static class CreateGeography
{
    #region 创建天气信息
    /// <summary>
    /// 创建一个天气信息
    /// </summary>
    /// <param name="area">天气的地区</param>
    /// <param name="period">天气的时间段</param>
    /// <param name="weatherType">天气类型</param>
    /// <param name="strength">天气的强度，从0开始，
    /// 例如小雨的强度是0，中雨的强度是1</param>
    /// <returns></returns>
    public static IWeatherInfo Weather(IAdministrativeArea area, IIntervalSpecific<DateTimeOffset> period,
        WeatherType weatherType, int strength)
        => new WeatherInfo(area, period)
        {
            WeatherType = weatherType,
            Strength = strength
        };
    #endregion
    #region 创建行政区
    /// <summary>
    /// 创建行政区
    /// </summary>
    /// <param name="area">行政区以及它的所有上级，
    /// 这个集合的第一个元素是国家，第二个元素（如果有）是一级行政区，
    /// 第三个元素是二级行政区，依此类推</param>
    /// <returns></returns>
    public static IAdministrativeArea Area(IReadOnlyList<string> area)
        => new AdministrativeArea(area);
    #endregion
    #region 创建地理位置
    /// <summary>
    /// 创建一个地理位置对象
    /// </summary>
    /// <param name="longitude">经度，正值表示东经，负值表示西经</param>
    /// <param name="latitude">纬度，正值表示北纬，负值表示南纬</param>
    /// <param name="description">对位置的描述</param>
    /// <param name="accuracy">定位的精度，以米为单位</param>
    public static ILocation Location(decimal longitude, decimal latitude, string? description = null, decimal accuracy = 50)
        => new Location
        {
            Longitude = longitude,
            Latitude = latitude,
            Accuracy = accuracy,
            Description = description
        };
    #endregion
    #region 创建地理范围
    #region 标准方法
    /// <summary>
    /// 创建一个圆形的地理范围
    /// </summary>
    /// <param name="position">地理范围的圆心位置</param>
    /// <param name="radius">地理范围的半径</param>
    /// <returns></returns>
    public static ILocationRangeCircle LocationRangeCircle(ILocation position, IUnit<IUTLength> radius)
        => new LocationRangeCircle()
        {
            Position = position,
            Radius = radius
        };
    #endregion
    #region 直接创建
    /// <param name="radius">地理位置的半径，以米为单位</param>
    /// <inheritdoc cref="Location(decimal, decimal, string?, decimal)"/>
    public static ILocationRangeCircle LocationRangeCircle(decimal longitude, decimal latitude,
        decimal radius, string? description = null, decimal accuracy = 50)
    {
        ExceptionIntervalOut.Check(0M, null, radius);
        var position = Location(longitude, latitude, description, accuracy);
        var length = CreateBaseMath.Unit(radius, IUTLength.MetersMetric);
        return LocationRangeCircle(position, length);
    }
    #endregion
    #endregion
}
