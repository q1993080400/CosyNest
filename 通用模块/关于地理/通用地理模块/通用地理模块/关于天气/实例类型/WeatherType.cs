namespace System.Geography.Weather;

/// <summary>
/// 这个枚举表示天气的类型
/// </summary>
public enum WeatherType
{
    /// <summary>
    /// 表示一个晴好的天气，强度0代表阴天，
    /// 强度1代表多云，强度2代表晴天
    /// </summary>
    Sunny,
    /// <summary>
    /// 下雨
    /// </summary>
    Rain,
    /// <summary>
    /// 下雪
    /// </summary>
    Snow
}
