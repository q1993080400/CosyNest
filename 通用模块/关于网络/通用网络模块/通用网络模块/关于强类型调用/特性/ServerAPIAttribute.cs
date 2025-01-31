namespace System.NetFrancis;

/// <summary>
/// 这个特性指示某个接口是强类型服务端API，
/// 在服务端上使用强类型调用调用它的时候，
/// 不应该发起Http请求，而是将它转发给内部的服务实现
/// </summary>
[AttributeUsage(AttributeTargets.Interface, Inherited = true)]
public sealed class ServerAPIAttribute : Attribute
{
}
