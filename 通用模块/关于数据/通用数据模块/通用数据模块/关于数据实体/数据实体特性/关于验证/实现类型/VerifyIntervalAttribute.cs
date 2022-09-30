namespace System.DataFrancis.Verify;

/// <summary>
/// 这个特性是一个区间验证，
/// 只有当对象位于指定的区间中，才能通过验证
/// </summary>
public sealed class VerifyIntervalAttribute : VerifyAttribute
{
    #region 公开成员
    #region 最大值
    /// <summary>
    /// 获取区间的最大值，
    /// 如果为<see cref="double.MaxValue"/>，表示没有最大值
    /// </summary>
    public double Max { get; }
    #endregion
    #region 最小值
    /// <summary>
    /// 获取区间的最小值，
    /// 如果为<see cref="double.MinValue"/>，表示没有最小值
    /// </summary>
    public double Min { get; }
    #endregion
    #region 执行验证
    public override (bool IsSuccess, string Message) Verify(object? obj, string? describe = null)
    {
        describe ??= "数据";
        var num = obj.To<double?>(false);
        if (num is null)
            return (false, $"{describe}不是数字类型，验证不通过");
        var isSuccess = IInterval.CheckInInterval(num.Value, Min, Max) is IntervalPosition.Located;
        return (isSuccess,
            isSuccess ?
            "" :
            $"{describe}必须" +
            (Min is double.MinValue ? "" : $"大于等于{Min}") +
            ((Min, Max) is ({ }, { }) ? "，且" : "") +
            (Max is double.MaxValue ? "" : $"小于等于{Max}"));
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的最小值和最大值初始化对象
    /// </summary>
    /// <param name="min">区间的最小值，
    /// 如果为<see langword="null"/>，表示没有最小值</param>
    /// <param name="max">区间的最大值，
    /// 如果为<see langword="null"/>，表示没有最大值</param>
    public VerifyIntervalAttribute(double min = double.MinValue, double max = double.MaxValue)
    {
        Min = min;
        Max = max;
    }
    #endregion
}
