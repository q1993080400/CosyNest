using System.Reflection;

namespace System.DataFrancis.EntityDescribe;

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
    /// 这个集合的元素是一个元组，
    /// 它的第一个项是验证失败的属性，
    /// 第二个项是验证失败的原因，
    /// 如果为空集合，表示验证成功
    /// </summary>
    public required IReadOnlyList<(PropertyInfo Property, string Prompt)> FailureReason { get; init; }
    #endregion
    #region 验证错误信息
    /// <summary>
    /// 获取验证错误信息，
    /// 如果验证成功，
    /// 则为<see langword="null"/>
    /// </summary>
    public string? ErrorMessage
        => IsSuccess ?
        null :
        FailureReason.Join(x => x.Prompt, Environment.NewLine);
    #endregion
    #region 是否验证成功
    /// <summary>
    /// 获取是否验证成功
    /// </summary>
    public bool IsSuccess
        => !FailureReason.Any();
    #endregion
}
