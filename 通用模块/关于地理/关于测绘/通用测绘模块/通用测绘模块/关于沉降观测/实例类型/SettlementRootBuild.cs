using System.MathFrancis;

namespace System.Mapping.Settlement;

/// <summary>
/// 该记录被用来生成沉降观测的起始点
/// </summary>
public sealed record SettlementRootBuild
{
    #region 名称
    /// <summary>
    /// 获取起始点的名称
    /// </summary>
    public string Name { get; init; }
    #endregion
    #region 关于已知点
    #region 索引已知点
    private Dictionary<string, IUnit<IUTLength>>? KnownField;

    /// <inheritdoc cref="SettlementPointRoot.Known"/>
    public Dictionary<string, IUnit<IUTLength>> Known
    {
        get => KnownField ??= [];
        init => KnownField = new(value);
    }
    #endregion
    #region 添加已知点，高程以米为单位
    /// <summary>
    /// 向<see cref="Known"/>中添加已知点，
    /// 它的高程以米为单位，然后返回这个对象本身
    /// </summary>
    /// <param name="name">已知点的名称</param>
    /// <param name="high">已知点的高程，以米为单位</param>
    /// <returns></returns>
    public SettlementRootBuild AddKnow(string name, Num high)
    {
        Known.Add(name, CreateBaseMath.Unit(high, IUTLength.MetersMetric));
        return this;
    }
    #endregion
    #endregion
    #region 高程
    /// <summary>
    /// 获取起始点的高程，如果它不存在于<see cref="Known"/>中，
    /// 则判定处于反向测量模式，高程统一指定为20米
    /// </summary>
    public IUnit<IUTLength> High
        => Known.TryGetValue(Name).Value ??
        CreateBaseMath.UnitMetric<IUTLength>(20);
    #endregion
    #region 关于转点
    #region 判断转点的委托
    /// <inheritdoc cref="SettlementPointRoot.IsIntermediaryDelegate"/>
    public Func<string, bool> IsIntermediary { get; init; }
        = static x => x.StartsWith("tp", StringComparison.CurrentCultureIgnoreCase);
    #endregion
    #region 创建转点名称的委托
    /// <inheritdoc cref="SettlementPointRoot.CreateTPNameDelegate"/>
    public Func<int, string> CreateTPName { get; init; } = static x => $"TP{x}";
    #endregion
    #endregion
}
