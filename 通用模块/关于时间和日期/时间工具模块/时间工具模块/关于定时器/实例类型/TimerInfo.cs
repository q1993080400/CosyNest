namespace System.TimeFrancis;

/// <summary>
/// 这个记录是定时器委托的返回值
/// </summary>
public sealed record TimerInfo
{
    #region 等待定时器的Task
    /// <summary>
    /// 获取一个<see cref="Task"/>，
    /// 等待它就可以等待定时器的下一个周期，
    /// 如果为<see langword="true"/>，表示正常到期，
    /// 如果为<see langword="false"/>，表示中途取消
    /// </summary>
    public required Task<bool> Wait { get; init; }
    #endregion
    #region 定时器下一次触发的日期
    /// <summary>
    /// 获取定时器下一次触发的日期，
    /// 如果不会有下一次，则返回<see langword="null"/>
    /// </summary>
    public required DateTimeOffset? Next { get; init; }
    #endregion
}
