namespace System.NetFrancis;

/// <summary>
/// 这个枚举指示一个强类型Http调用的参数，
/// 应该通过什么方式封装到调用中
/// </summary>
public enum HttpInvokeParameterSource
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
