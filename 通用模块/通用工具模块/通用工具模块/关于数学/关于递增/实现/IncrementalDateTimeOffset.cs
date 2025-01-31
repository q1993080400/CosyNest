namespace System.MathFrancis;

/// <summary>
/// 这个对象是<see cref="IIncremental{Obj}"/>的实现，
/// 可以返回一个递增的时间
/// </summary>
/// <param name="dateTimeOffset">初始时间</param>
sealed class IncrementalDateTimeOffset(DateTimeOffset dateTimeOffset) : IIncremental<DateTimeOffset>
{
    #region 获取递增的时间
    public DateTimeOffset Incremental()
         => dateTimeOffset = dateTimeOffset.AddMilliseconds(1);
    #endregion
}
