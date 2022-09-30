using System.Maths;
using System.Maths.Tree;

namespace System.Mapping.Settlement;

/// <summary>
/// 这个类型是<see cref="ISettlementPoint"/>的实现，
/// 可以作为一个沉降观测点
/// </summary>
sealed class SettlementPoint : SettlementPointBase
{
    #region 关于高程
    #region 已知高程
    /// <summary>
    /// 返回这个点已知的固定高程，
    /// 如果它不是已知的，则返回<see langword="null"/>
    /// </summary>
    private IUnit<IUTLength>? HighKnown { get; }
    #endregion
    #region 原始高程
    /// <summary>
    /// 获取原始高程，
    /// 它直接经过计算得出，未经平差，也不考虑固定点的情况
    /// </summary>
    private IUnit<IUTLength> HighOriginal
        => Father.To<ISettlement>()!.High - Recording!;
    #endregion
    #region 正式高程
    public override IUnit<IUTLength> High
        => HighKnown ??
        Closed?.High ??
        HighOriginal + ClosedDifference;
    #endregion
    #endregion
    #region 是否为已知点
    public override bool IsKnown
        => HighKnown is { } || Closed is { };
    #endregion
    #region 关于转点
    #region 是否为转点
    public override bool IsIntermediary
        => Ancestors.IsIntermediaryDelegate(Name);
    #endregion
    #region 创建转点名称
    public override string CreateTPName(int index)
        => Ancestors.CreateTPName(index);
    #endregion
    #endregion
    #region 关于闭合/附合
    #region 闭合/附合点 
    private ISettlementPoint? ClosedField;

    public override ISettlementPoint? Closed => ClosedField;
    #endregion
    #region 刷新闭合
    /// <summary>
    /// 调用这个方法以刷新闭合站和闭合差
    /// </summary>
    internal void RefreshClosed()
    {
        #region 用于枚举闭合环的本地函数
        SettlementBase[] Closed()
        {
            var list = new LinkedList<SettlementBase>();
            foreach (var item in this.To<INode>().AncestorsAll().OfType<SettlementBase>())
            {
                list.AddLast(item);
                if (item is SettlementPointBase p && (p.Name == Name || (IsKnown && p.IsKnown)))        //检查是否为闭合或附合
                    return list.ToArray();
            }
            return CreateCollection.EmptyArray(list);
        }
        #endregion
        var closed = Closed();
        if (closed.Any())
        {
            ClosedField = (ISettlementPoint)closed[^1];
            var settlements = closed[..^1].Append(this).ToArray();
            settlements.ForEach(x => x.ClosedDifference = IUnit<IUTLength>.Zero);
            var difference = (this.Closed!.High - HighOriginal) / (closed.Length / 2);
            settlements.ForEach(x => x.ClosedDifference = difference);
        }
    }
    #endregion
    #endregion
    #region 移除所有后代
    public override void RemoveOffspring()
    {
        SonField.ForEach(x => x.Father = null);
        SonField.Clear();
        #region 用于清除闭合差的本地函数
        static void Fun(INode? node)
        {
            switch (node)
            {
                case SettlementPoint p:
                    if (p.ClosedDifference.Equals(IUnit<IUTLength>.Zero) || p.Closed is { })
                        return;
                    p.ClosedDifference = IUnit<IUTLength>.Zero;
                    Fun(p.Father);
                    break;
                case SettlementObservatory o:
                    Fun(o.Father);
                    break;
            }
        }
        #endregion
        Fun(this);
    }
    #endregion
    #region 构造函数
    #region 创建已知点
    /// <summary>
    /// 创建一个已知点，它的高程已经确定
    /// </summary>
    /// <param name="recording">记录</param>
    /// <param name="high">已知高程</param>
    /// <param name="father">父观测站</param>
    /// <inheritdoc cref="SettlementPointBase(string)"/>
    public SettlementPoint(string name, IUnit<IUTLength>? recording,
        IUnit<IUTLength> high, ISettlementObservatory father)
        : base(name)
    {
        this.Recording = recording;
        this.Father = father;
        this.HighKnown = high;
        RefreshClosed();
        if (Direction is SettlementDirection.ReverseUndone)
            Ancestors.ConnectKnow(HighOriginal - HighKnown);
    }
    #endregion
    #region 创建普通点
    /// <summary>
    /// 创建一个普通沉降点，它的高程需要计算得出
    /// </summary>
    /// <inheritdoc cref="SettlementPoint(string, IUnit{IUTLength}?, IUnit{IUTLength}, ISettlementObservatory)"/>
    public SettlementPoint(string name, IUnit<IUTLength>? recording,
        ISettlementObservatory father)
        : base(name)
    {
        this.Recording = recording;
        this.Father = father;
        RefreshClosed();
    }
    #endregion
    #endregion
}
