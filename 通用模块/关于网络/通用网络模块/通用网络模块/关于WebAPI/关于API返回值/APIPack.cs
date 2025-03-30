namespace System.NetFrancis.Api;

/// <summary>
/// 这个记录可以用来指示一个API是否成功完成，
/// 如果未成功完成，还可以指示失败的原因
/// </summary>
public record APIPack : IHasResult
{
    #region 是否成功
    /// <summary>
    /// 获取请求是否成功
    /// </summary>
    public bool Success => FailureReason.IsVoid() || IgnoreExceptions;
    #endregion
    #region 是否应忽略异常
    /// <summary>
    /// 获取是否应该忽略异常，
    /// 如果它为<see langword="true"/>，
    /// 即便<see cref="FailureReason"/>不为<see langword="null"/>，
    /// 仍然视为请求成功，<see cref="FailureReason"/>仅作为提示
    /// </summary>
    public bool IgnoreExceptions { get; init; }
    #endregion
    #region 请求失败的原因
    /// <summary>
    /// 如果这个请求成功，则为<see langword="null"/>，
    /// 如果失败，则指示失败的原因
    /// </summary>
    public string? FailureReason { get; init; }
    #endregion
}
