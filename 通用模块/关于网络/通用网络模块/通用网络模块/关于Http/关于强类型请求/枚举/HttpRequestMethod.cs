namespace System.NetFrancis.Http;

/// <summary>
/// 这个枚举指示一个强类型Http请求，
/// 应该使用哪个Http方法
/// </summary>
public enum HttpRequestMethod
{
    /// <summary>
    /// 自动判断
    /// </summary>
    Auto,
    /// <summary>
    /// 使用Get方法
    /// </summary>
    Get,
    /// <summary>
    /// 使用Post方法
    /// </summary>
    Post
}
