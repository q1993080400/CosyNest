namespace System.NetFrancis.Api;

/// <summary>
/// 该类型表示请求WebAPI的过程中引发的异常
/// </summary>
/// <inheritdoc cref="Exception(string?)"/>
public sealed class APIException(string? message) : Exception(message)
{

}
