namespace System.DataFrancis;

/// <summary>
/// 这个类型表示由于数据验证失败所导致的异常
/// </summary>
/// <inheritdoc cref="BusinessException(string?)"/>
public sealed class ValidationException(string? message) : BusinessException(message)
{

}
