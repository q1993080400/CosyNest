namespace System.Mapping.Settlement;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个沉降观测站
/// </summary>
public interface ISettlementObservatory : ISettlement
{
    #region 父代观测点
    /// <summary>
    /// 获取此观测站前视的观测点
    /// </summary>
    new ISettlementPoint Father
        => (ISettlementPoint)Base.Father!;
    #endregion
    #region 子代观测站
    /// <summary>
    /// 枚举此观测站后视的所有观测点
    /// </summary>
    new IEnumerable<ISettlementPoint> Son
        => Base.Son.Cast<ISettlementPoint>();
    #endregion
}
