namespace System.DataFrancis;

/// <summary>
/// 这个记录表示验证数据的结果
/// </summary>
public sealed record VerificationResults
{
    #region 验证数据
    /// <summary>
    /// 获取进行验证的数据
    /// </summary>
    public required object Data { get; init; }
    #endregion
    #region 验证失败的原因
    /// <summary>
    /// 枚举验证失败的原因，
    /// 如果为空集合，表示验证成功
    /// </summary>
    public required IReadOnlyCollection<FailureReason> FailureReason { get; init; }
    #endregion
    #region 验证失败的消息
    /// <summary>
    /// 获取验证失败的消息，
    /// 它是由<see cref="FailureReason"/>中的所有消息连接而成
    /// </summary>
    public string FailureReasonMessage()
        => FailureReason.Join(static x => x.Prompt, Environment.NewLine);
    #endregion
    #region 是否验证成功
    /// <summary>
    /// 获取是否验证成功
    /// </summary>
    public bool IsSuccess
        => FailureReason.Count is 0;
    #endregion
}
