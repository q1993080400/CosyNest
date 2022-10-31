using System.Geography.Map;
using System.Geography.Weather;
using System.NetFrancis.Http;

namespace System.Geography;

/// <summary>
/// 这个静态类可以用来创建和地理有关的WebAPI
/// </summary>
public static class CreateGeographicAPI
{
    #region 创建天气接口
    #region 易源天气接口
    /// <summary>
    /// 创建易源天气接口
    /// </summary>
    /// <inheritdoc cref="WeatherYiYuan.WeatherYiYuan(string, Func{IHttpClient}?)"/>
    /// <returns></returns>
    public static IWeather WeatherYiYuan(string appCode, Func<IHttpClient>? httpClientProvide = null)
        => new WeatherYiYuan(appCode, httpClientProvide);
    #endregion
    #endregion
    #region 创建地图接口
    #region 创建百度地图接口
    /// <summary>
    /// 创建一个使用百度地图实现的地图接口
    /// </summary>
    /// <inheritdoc cref="BaiduMap(string, Func{Task{ILocation?}}?, Func{IHttpClient}?)"/>
    public static IMap MapBaiDu(string ak, Func<Task<ILocation?>>? position = null, Func<IHttpClient>? httpClientProvide = null)
        => new BaiduMap(ak, position, httpClientProvide);
    #endregion
    #endregion

}
