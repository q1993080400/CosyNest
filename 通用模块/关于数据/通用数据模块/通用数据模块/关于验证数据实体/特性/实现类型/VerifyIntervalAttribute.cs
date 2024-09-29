namespace System.DataFrancis.EntityDescribe;

/// <summary>
/// 这个特性是一个区间验证，
/// 只有当对象位于指定的区间中，才能通过验证
/// </summary>
/// <remarks>
/// 用指定的最小值和最大值初始化对象
/// </remarks>
/// <param name="min">区间的最大值，如果它不能转换为数字，表示没有最大值</param>
/// <param name="max">区间的最小值，如果它不能转换为数字，说明没有最小值</param>
public sealed class VerifyIntervalAttribute(string? min = null, string? max = null) : VerifyAttribute
{
    #region 公开成员
    #region 最大值
    /// <summary>
    /// 获取区间的最大值
    /// </summary>
    public decimal Max { get; } = max.To<decimal>(false, new(decimal.MaxValue));
    #endregion
    #region 最小值
    /// <summary>
    /// 获取区间的最小值
    /// </summary>
    public decimal Min { get; } = min.To<decimal>(false, new(decimal.MinValue));
    #endregion
    #region 执行验证
    public override string? Verify(object? obj, string describe)
    {
        var num = obj.To<decimal?>(false);
        if (num is null)
            return $"{describe}不是数字类型，验证不通过";
        var isSuccess = IInterval.CheckInInterval(num.Value, Min, Max) is IntervalPosition.Located;
        return isSuccess ?
            null :
            $"{describe}必须{(Min, Max) switch
            {
                ( > decimal.MinValue, < decimal.MaxValue) => $"大于等于{Min}且小于等于{Max}",
                ( > decimal.MinValue, _) => $"大于等于{Min}",
                (_, < decimal.MaxValue) => $"小于等于{Max}",
                _ => throw new NotSupportedException("无法处理这种数字范围模式")
            }}";
    }

    #endregion
    #endregion
}
