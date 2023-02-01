namespace System.NetFrancis.Http;

/// <summary>
/// 这个特性表示在强类型Http请求中，
/// 对于接口方法应使用Post
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class HttpMethodPostAttribute : Attribute
{

}
