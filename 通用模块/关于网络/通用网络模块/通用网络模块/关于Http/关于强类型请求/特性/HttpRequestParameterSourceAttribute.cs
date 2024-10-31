namespace System.NetFrancis.Http;

/// <summary>
/// 这个特性指示一个强类型Http请求的参数，
/// 应该通过什么方式封装到请求中
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class HttpRequestParameterSourceAttribute : Attribute
{
    #region 指定参数来源
    /// <summary>
    /// 指定强类型Http请求的参数，
    /// 应该通过什么方式封装到请求中
    /// </summary>
    public HttpRequestParameterSource ParameterSource { get; init; }
    #endregion
}
