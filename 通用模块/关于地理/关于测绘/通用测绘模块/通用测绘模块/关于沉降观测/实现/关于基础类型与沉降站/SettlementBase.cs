using System.Maths;
using System.Maths.Tree;

namespace System.Mapping.Settlement;

/// <summary>
/// 这个类型是所有沉降观测的抽象类型
/// </summary>
abstract class SettlementBase : ISettlement
{
    #region 测量方向
    public virtual SettlementDirection Direction => Ancestors.Direction;
    #endregion
    #region 记录
    private IUnit<IUTLength>? RecordingField;

    public virtual IUnit<IUTLength>? Recording
    {
        get => RecordingField;
        set
        {
            if (value is null && RecordingField is { })
                throw new NotSupportedException("记录不允许写入null，这毫无意义");
            var notFirstSet = RecordingField is { };
            RecordingField = value;
            RecordingField = value;
            if (notFirstSet && ClosedDifference.ValueMetric != 0)
                this.To<ISettlement>().SonAll.OfType<SettlementPoint>().
                    Where(x => x.Closed is { }).ForEach(x => x.RefreshClosed());
        }
    }
    #endregion
    #region 高程
    public abstract IUnit<IUTLength> High { get; }
    #endregion
    #region 闭合/附合差
    public virtual IUnit<IUTLength> ClosedDifference { get; internal set; } = IUnit<IUTLength>.Zero;
    #endregion
    #region 关于父节点和祖先节点
    #region 父节点
    public INode? Father { get; internal set; }
    #endregion
    #region 基准点
    private SettlementPointRoot? AncestorsField;

    /// <inheritdoc cref="ISettlement.Ancestors"/>
    protected virtual SettlementPointRoot Ancestors
        => AncestorsField ??= (SettlementPointRoot)Father!.Ancestors;

    INode INode.Ancestors => Ancestors;
    #endregion
    #endregion
    #region 子节点
    /// <summary>
    /// 枚举该沉降观测点或观测站的所有后代
    /// </summary>
    protected LinkedList<SettlementBase> SonField { get; } = new();

    public IEnumerable<INode> Son => SonField;
    #endregion
}
