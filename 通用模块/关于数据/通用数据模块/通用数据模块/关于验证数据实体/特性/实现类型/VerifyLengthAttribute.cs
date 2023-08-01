namespace System.DataFrancis.EntityDescribe;

/// <summary>
/// 这个验证特性要求文本具有指定范围的长度
/// </summary>
public sealed class VerifyLengthAttribute : VerifyAttribute
{
    #region 最大长度
    /// <summary>
    /// 获取文本所允许的最大长度，
    /// 如果不指定，表示没有限制
    /// </summary>
    public int MaxLength { get; init; } = int.MaxValue;
    #endregion
    #region 最小长度
    /// <summary>
    /// 获取文本所允许的最小长度，
    /// 如果不指定，表示没有限制
    /// </summary>
    public int MinLength { get; init; } = 0;
    #endregion
    #region 执行验证
    public override string? Verify(object? obj, string describe)
    {
        if (obj is not string { Length: var len } t)
            return $"{describe}不是不为null的string，无法验证";
        var isSuccess = IInterval.CheckInInterval(len, MinLength, MaxLength) is IntervalPosition.Located;
        return isSuccess ?
            null :
            $"{describe}的长度必须{(MinLength, MaxLength) switch
            {
                ( > 0, < int.MaxValue) => $"大于等于{MinLength}且小于等于{MaxLength}",
                ( > 0, _) => $"大于等于{MinLength}",
                (_, < int.MaxValue) => $"小于等于{MaxLength}",
                _ => throw new NotSupportedException("无法处理这种文本长度范围模式")
            }}";
    }
    #endregion
}
