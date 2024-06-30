namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来描述一个筛选目标
/// </summary>
public interface IHasFilterTarget : IHasFilterIdentification
{
    #region 是否为虚拟筛选
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示它是一个虚拟筛选，不直接映射到某个具体的属性上，
    /// 它还会影响<see cref="IHasFilterIdentification.Identification"/>的意义
    /// </summary>
    bool IsVirtually { get; }
    #endregion
}
