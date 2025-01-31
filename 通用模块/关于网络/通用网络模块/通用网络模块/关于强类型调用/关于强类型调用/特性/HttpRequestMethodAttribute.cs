namespace System.NetFrancis;

/// <summary>
/// 这个特性指示一个强类型Http请求，
/// 应该使用哪个Http方法
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class HttpRequestMethodAttribute : Attribute
{
    #region 指定应该使用的Http方法
    /// <summary>
    /// 应该使用的Http方法
    /// </summary>
    public HttpInvokeMethod Method { get; init; }
    #endregion
}
