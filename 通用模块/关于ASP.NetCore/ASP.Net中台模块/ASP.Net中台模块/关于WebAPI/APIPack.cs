namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录可以用来指示一个API是否成功完成，
/// 如果未成功完成，还可以指示失败的原因
/// </summary>
public record APIPack
{
    #region 是否成功
    /// <summary>
    /// 指示这个请求是否成功
    /// </summary>
    public bool IsSuccess => FailureReason.IsVoid();
    #endregion
    #region 请求失败的原因
    /// <summary>
    /// 如果这个请求成功，则为<see langword="null"/>，
    /// 如果失败，则指示失败的原因
    /// </summary>
    public string? FailureReason { get; init; }
    #endregion
    #region 状态码
    /// <summary>
    /// 这个请求的状态码
    /// </summary>
    public int? StatusCode { get; init; }
    #endregion
}
