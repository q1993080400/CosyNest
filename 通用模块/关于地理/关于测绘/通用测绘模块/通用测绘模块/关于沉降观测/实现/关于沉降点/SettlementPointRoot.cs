using System.Maths;

namespace System.Mapping.Settlement;

/// <summary>
/// 这个类型是<see cref="ISettlementPoint"/>的实现，
/// 可以视为一个专门用来作为起始点的沉降观测点
/// </summary>
sealed class SettlementPointRoot : SettlementPointBase
{
    #region 索引高程已知的沉降点
    /// <summary>
    /// 索引本次沉降观测中，
    /// 高程已知的点的名称的高程
    /// </summary>
    public IReadOnlyDictionary<string, IUnit<IUTLength>> Known { get; }
    #endregion
    #region 关于测量方向
    #region 通知已连接至已知点
    /// <summary>
    /// 在反向测量且已经连接到一个已知点时，
    /// 调用本方法向起始点告知虚拟高程差，
    /// 使其可以获取真实的高程
    /// </summary>
    /// <param name="virtualDifference">虚拟高程差，
    /// 它等于虚拟高程减去已知点的实际高程</param>
    internal void ConnectKnow(IUnit<IUTLength> virtualDifference)
    {
        if (Direction is SettlementDirection.ReverseUndone)
        {
            DirectionField = SettlementDirection.Reverse;
            this.VirtualDifference = virtualDifference;
        }
    }
    #endregion
    #region 获取测量方向
    private SettlementDirection DirectionField;

    public override SettlementDirection Direction => DirectionField;
    #endregion
    #endregion
    #region 高程
    #region 虚拟高程与实际高程的差
    /// <summary>
    /// 获取虚拟高程与实际高程的差，
    /// 通过它可以在反向测量中获取真正的高程
    /// </summary>
    private IUnit<IUTLength> VirtualDifference { get; set; } = IUnit<IUTLength>.Zero;
    #endregion
    #region 正式方法
    private readonly IUnit<IUTLength> HighField;

    public override IUnit<IUTLength> High => HighField - VirtualDifference;
    #endregion
    #endregion
    #region 记录
    public override IUnit<IUTLength>? Recording
    {
        get => null;
        set { }
    }
    #endregion
    #region 返回基准点
    protected override SettlementPointRoot Ancestors => this;
    #endregion
    #region 关于转点
    #region 判断转点的委托
    /// <summary>
    /// 该委托传入点的名称，然后判断是否为转点
    /// </summary>
    internal Func<string, bool> IsIntermediaryDelegate { get; }
    #endregion
    #region 是否转点
    public override bool IsIntermediary => false;
    #endregion
    #region 创建转点名称的委托
    /// <summary>
    /// 该委托传入转点的编号，返回转点的名称
    /// </summary>
    private Func<int, string> CreateTPNameDelegate { get; }
    #endregion
    #region 创建转点名称
    public override string CreateTPName(int index)
        => CreateTPNameDelegate(index);
    #endregion
    #endregion
    #region 是否已知点
    public override bool IsKnown
        => Direction is SettlementDirection.Positive;
    #endregion
    #region 关于闭合与附合
    #region 闭合/附合点
    public override ISettlementPoint? Closed => null;
    #endregion
    #region 闭合/附合差
    public override IUnit<IUTLength> ClosedDifference
    {
        get => IUnit<IUTLength>.Zero;
        internal set
        {

        }
    }
    #endregion
    #endregion
    #region 清除后代
    public override void RemoveOffspring()
    {
        SonField.ForEach(x => x.Father = null);
        SonField.Clear();
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 通过生成器创建起始点
    /// </summary>
    /// <param name="build">该对象封装了创建起始点所需要的数据</param>
    public SettlementPointRoot(SettlementRootBuild build)
        : base(build.Name)
    {
        this.Known = build.Known.ToDictionary(true);
        this.IsIntermediaryDelegate = build.IsIntermediary;
        this.CreateTPNameDelegate = build.CreateTPName;
        this.HighField = build.High;
        DirectionField = Known.ContainsKey(Name) ? SettlementDirection.Positive : SettlementDirection.ReverseUndone;
    }
    #endregion
}
