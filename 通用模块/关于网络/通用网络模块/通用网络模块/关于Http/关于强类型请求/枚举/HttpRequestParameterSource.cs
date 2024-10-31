namespace System.NetFrancis.Http;

/// <summary>
/// 这个枚举指示一个强类型Http请求的参数，
/// 应该通过什么方式封装到请求中
/// </summary>
public enum HttpRequestParameterSource
{
    /// <summary>
    /// 自动判断
    /// </summary>
    Auto,
    /// <summary>
    /// 表示参数来自Uri查询字符串
    /// </summary>
    FromQuery,
    /// <summary>
    /// 表示参数来自请求体
    /// </summary>
    FromBody
}
