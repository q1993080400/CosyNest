namespace System.NetFrancis.Api;

/// <summary>
/// 该类型表示请求WebAPI的过程中引发的异常
/// </summary>
public sealed class APIException : Exception
{
    #region 构造函数
    /// <inheritdoc cref="Exception(string?)"/>
    public APIException(string? message)
        : base(message)
    {
    }
    #endregion 
}
