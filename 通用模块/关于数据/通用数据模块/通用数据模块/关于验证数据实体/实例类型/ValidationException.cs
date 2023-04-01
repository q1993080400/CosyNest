using System.Runtime.Serialization;

namespace System.DataFrancis.Verify;

/// <summary>
/// 这个类型表示由于数据验证失败所导致的异常
/// </summary>
public sealed class ValidationException : BusinessException
{
    #region 构造函数
    /// <inheritdoc cref="BusinessException(string?)"/>
    public ValidationException(string? message)
        : base(message)
    {

    }

    /// <inheritdoc cref="BusinessException(SerializationInfo, StreamingContext))"/>
    public ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }

    /// <inheritdoc cref="BusinessException(string?, Exception?)"/>
    public ValidationException(string? message, Exception? innerException) : base(message, innerException)
    {

    }
    #endregion 
}
