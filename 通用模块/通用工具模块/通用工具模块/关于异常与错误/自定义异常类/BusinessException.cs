namespace System;

/// <summary>
/// 这个类型表示一个业务异常
/// </summary>
/// <inheritdoc cref="Exception(string?)"/>
public class BusinessException(string? message) : Exception(message)
{

}
