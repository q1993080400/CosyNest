namespace System.TimeFrancis;

/// <summary>
/// 这个类型是一个限制次数的定时器，
/// 当原始定时器触发到指定的次数时，会自动停止
/// </summary>
/// <param name="timer">原始定时器</param>
/// <param name="maxCount">定时器触发的最大次数</param>
sealed class TimerLimitFrequency(Timer timer, int maxCount)
{
    #region 公开成员
    #region 等待定时器到期
    /// <inheritdoc cref="TimeFrancis.Timer"/>
    public TimerInfo? Timer(CancellationToken cancellationToken = default)
    {
        if (maxCount <= 0 || cancellationToken.IsCancellationRequested)
            return null;
        var info = timer(cancellationToken);
        maxCount--;
        return (info, maxCount) switch
        {
            (null, _) => null,
            ({ }, <= 0) => info with
            {
                Next = null
            },
            _ => info
        };
    }
    #endregion
    #endregion
}
