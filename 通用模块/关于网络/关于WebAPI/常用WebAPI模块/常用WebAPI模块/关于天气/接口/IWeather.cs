
using System.Geography;

namespace System.NetFrancis.Api.Weather;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来请求天气预报和历史天气
/// </summary>
public interface IWeather
{
    #region 请求天气
    /// <summary>
    /// 请求某一地区在某一个时间区间中的天气
    /// </summary>
    /// <param name="area">地区</param>
    /// <param name="date">时间区间，如果为<see langword="null"/>，则默认为未来24小时</param>
    /// <returns>返回的天气，如果返回多个值，代表这一时间段中更小的时间段的天气，
    /// 例如某一天是晴转多云，就返回晴和多云两个天气</returns>
    /// <param name="cancellation">一个用于取消异步操作的令牌</param>
    IAsyncEnumerable<IWeatherInfo> GetWeathers(IAdministrativeArea area, IIntervalSpecific<DateTimeOffset>? date = null, CancellationToken cancellation = default);
    #endregion
}
