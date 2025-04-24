namespace System.TimeFrancis;

/// <summary>
/// 这个记录是定时器委托的返回值
/// </summary>
public sealed record TimerInfo
{
    #region 等待定时器的Task
    /// <summary>
    /// 获取调用这个委托可以获取一个<see cref="Task"/>，
    /// 等待它就可以等待定时器的下一个周期，
    /// 如果为<see langword="true"/>，表示正常到期，
    /// 如果为<see langword="false"/>，表示中途取消
    /// </summary>
    public required Func<Task<bool>> Wait { get; init; }
    #endregion
    #region 定时器下一次触发的状态
    /// <summary>
    /// 返回一个元组，
    /// 它的第一个项指示是否存在下一次触发，
    /// 第二个项指示如果存在下一次触发，
    /// 而且时间确定，则返回下一次触发的时间
    /// </summary>
    public required (NextTimeState State, DateTimeOffset NextDate) NextTimeState { get; init; }
    #endregion
}
