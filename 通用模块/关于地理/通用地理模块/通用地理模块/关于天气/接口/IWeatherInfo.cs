namespace System.Geography;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个天气信息
/// </summary>
public interface IWeatherInfo
{
    #region 天气类型
    /// <summary>
    /// 获取天气类型
    /// </summary>
    WeatherType WeatherType { get; }
    #endregion
    #region 天气强度
    /// <summary>
    /// 获取天气的强度，从0开始，
    /// 例如阵雨的强度是0，小雨的强度是1
    /// </summary>
    int Strength { get; }
    #endregion
    #region 天气描述
    /// <summary>
    /// 返回对天气的描述
    /// </summary>
    string Describe => WeatherType switch
    {
        WeatherType.Sunny => Strength switch
        {
            0 => "阴天",
            1 => "多云",
            _ => "晴天"
        },
        WeatherType.Snow => Strength switch
        {
            0 => "阵雪",
            1 => "小雪",
            2 => "中雪",
            3 => "大雪",
            _ => "暴雪"
        },
        WeatherType.Rain => Strength switch
        {
            0 => "阵雨",
            1 => "小雨",
            2 => "中雨",
            3 => "大雨",
            _ => "暴雨"
        },
        var type => type.ToString()
    };
    #endregion
    #region 时间段
    /// <summary>
    /// 获取天气的时间段
    /// </summary>
    IIntervalSpecific<DateTimeOffset> Period { get; }
    #endregion
    #region 地区
    /// <summary>
    /// 获取天气的地区
    /// </summary>
    IAdministrativeArea Area { get; }
    #endregion
}
