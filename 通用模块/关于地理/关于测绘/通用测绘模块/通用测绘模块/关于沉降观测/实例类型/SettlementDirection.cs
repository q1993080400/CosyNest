namespace System.Mapping.Settlement;

/// <summary>
/// 该枚举表示沉降观测的方向
/// </summary>
public enum SettlementDirection
{
    /// <summary>
    /// 表示观测是按照正常的流程，
    /// 从一个已知点开始的
    /// </summary>
    Positive,
    /// <summary>
    /// 表示观测从一个未知点开始，
    /// 而且已经连接到至少一个已知点，
    /// 已经可以获取真实的高程
    /// </summary>
    Reverse,
    /// <summary>
    /// 表示观测从一个未知点开始，
    /// 但是尚未连接到一个已知点，
    /// 目前显示的高程是虚拟的
    /// </summary>
    ReverseUndone
}
