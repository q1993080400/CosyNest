namespace System.TimeFrancis;

/// <summary>
/// 这个枚举表示定时器下一次触发时间的状态
/// </summary>
public enum NextTimeState
{
    /// <summary>
    /// 表示这已经是定时器的最后一次触发，
    /// 不会有下一次触发
    /// </summary>
    NotNext,
    /// <summary>
    /// 表示确定存在下一次触发，
    /// 而且能够计算出确定的时间
    /// </summary>
    HasNext,
    /// <summary>
    /// 表示确定存在下一次触发，
    /// 但是出于各种原因，
    /// 无法计算出确定的时间
    /// </summary>
    Ambiguity
}
