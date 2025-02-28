namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个拥有状态变化时间的实体
/// </summary>
public interface IWithUpdateDate
{
    #region 状态发生变化的时间
    /// <summary>
    /// 获取状态发生变化的时间，
    /// 如果发现状态变化了，
    /// 可以提醒用户重新获取这个实体
    /// </summary>
    DateTimeOffset UpdateDate { get; }
    #endregion
}
