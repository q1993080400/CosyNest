namespace System.TimeFrancis;

/// <summary>
/// 该类型是<see cref="ITimer"/>的实现，
/// 可以视为一个定时器，它按照固定的周期触发
/// </summary>
sealed class TimerDefault : ITimer
{
    #region 定时器周期
    /// <summary>
    /// 返回定时器的触发周期
    /// </summary>
    public TimeSpan Cycle { get; init; }
    #endregion
    #region 立即触发一次周期
    private bool GoNowField;

    /// <summary>
    /// 如果这个属性为<see langword="true"/>，
    /// 则在第一次触发定时器时，无需等待，立即触发该周期
    /// </summary>
    public bool GoNow
    {
        get => GoNowField;
        init => GoNowField = value;
    }
    #endregion
    #region 是否激活
    public bool IsActivation { get; set; } = true;
    #endregion
    #region 返回等待定时器下一个周期的Task
    public Task Wait(CancellationToken cancellationToken = default)
    {
        if (!IsActivation)
            throw new NotSupportedException($"定时器处于冻结状态，无法执行这个操作");
        if (GoNow)
        {
            GoNowField = false;
            return Task.CompletedTask;
        }
        return Task.Delay(Cycle, cancellationToken);
    }
    #endregion
}
