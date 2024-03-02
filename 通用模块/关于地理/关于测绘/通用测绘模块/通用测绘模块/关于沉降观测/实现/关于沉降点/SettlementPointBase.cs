using System.MathFrancis;

namespace System.Mapping.Settlement;

/// <summary>
/// 该类型是<see cref="ISettlementPoint"/>的实现，
/// 同时也是沉降观测点的基类
/// </summary>
/// <remarks>
/// 使用指定的名称初始化沉降观测点
/// </remarks>
/// <param name="name">观测点的名称</param>
abstract class SettlementPointBase(string name) : SettlementBase, ISettlementPoint
{
    #region 名称
    public string Name { get; } = name;
    #endregion
    #region 是否已知
    public abstract bool IsKnown { get; }
    #endregion
    #region 关于转点
    #region 是否为转点
    public abstract bool IsIntermediary { get; }
    #endregion
    #region 创建转点名称
    public abstract string CreateTPName(int index);
    #endregion
    #endregion
    #region 关于后代
    #region 移除所有后代
    public abstract void RemoveOffspring();
    #endregion
    #region 添加后代观测站
    public ISettlementPoint Add(IUnit<IUTLength> frontRecording, string behindName, IUnit<IUTLength> behindRecording)
    {
        var son = new SettlementObservatory(frontRecording, this);
        SonField.AddLast(son);
        return son.AddPoint(behindName, behindRecording);
    }
    #endregion
    #endregion
    #region 返回闭合点
    public abstract ISettlementPoint? Closed { get; }
    #endregion
    #region 重写ToString
    public override string ToString() => Name;

    #endregion
}
