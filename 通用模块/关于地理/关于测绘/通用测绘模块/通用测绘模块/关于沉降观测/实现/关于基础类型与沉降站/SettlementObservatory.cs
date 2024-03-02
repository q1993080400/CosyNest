using System.MathFrancis;

namespace System.Mapping.Settlement;

/// <summary>
/// 这个类型代表一个沉降观测站
/// </summary>
sealed class SettlementObservatory : SettlementBase, ISettlementObservatory
{
    #region 高程
    public override IUnit<IUTLength> High
        => Father.To<ISettlement>()!.High + Recording!;
    #endregion
    #region 添加后代
    /// <summary>
    /// 添加一个后视此观测站的观测点
    /// </summary>
    /// <param name="name">观测点的名称</param>
    /// <param name="recording">观测点的记录</param>
    /// <returns></returns>
    internal ISettlementPoint AddPoint(string name, IUnit<IUTLength> recording)
    {
        var son = Ancestors.Known.TryGetValue(name, out var high) ?
            new SettlementPoint(name, recording, high, this) :
            new SettlementPoint(name, recording, this);
        SonField.AddLast(son);
        return son;
    }
    #endregion
    #region 重写ToString
    public override string ToString()
        => "这是沉降观测站";
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="recording">原始记录</param>
    /// <param name="father">父节点</param>
    public SettlementObservatory(IUnit<IUTLength> recording, ISettlementPoint father)
    {
        this.Recording = recording;
        this.Father = father;
    }
    #endregion
}
