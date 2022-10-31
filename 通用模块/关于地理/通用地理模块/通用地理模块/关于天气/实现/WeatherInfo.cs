namespace System.Geography.Weather;

/// <summary>
/// 这个记录表示某一地区某一时间段中的天气
/// </summary>
sealed record WeatherInfo : IWeatherInfo
{
    #region 公开成员
    #region 天气类型
    public WeatherType WeatherType { get; init; }
    #endregion
    #region 天气强度
    public int Strength { get; init; }
    #endregion
    #region 时间段
    private IIntervalSpecific<DateTimeOffset>? PeriodField;

    public IIntervalSpecific<DateTimeOffset> Period
    {
        get => PeriodField!;
        init => PeriodField = value.IsClosed ? value : throw new ArgumentException($"时间段必须是封闭区间");
    }
    #endregion
    #region 地区
    public IAdministrativeArea Area { get; init; }
    #endregion
    #region 重写ToString
    public override string ToString()
        => $"{Area.Area[^1]}在{Period.Min?.ToString("g")}的天气为{this.To<IWeatherInfo>().Describe}";
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="area">天气的地区</param>
    /// <param name="period">天气的时间段</param>
    public WeatherInfo(IAdministrativeArea area, IIntervalSpecific<DateTimeOffset> period)
    {
        this.Period = period;
        this.Area = area;
    }
    #endregion
}
