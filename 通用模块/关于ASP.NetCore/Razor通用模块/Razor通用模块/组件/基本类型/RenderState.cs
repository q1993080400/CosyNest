namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该记录描述了组件在呈现过程中所处的状态
/// </summary>
public sealed record RenderState : IComparable<RenderState>
{
    #region 运算符重载
    public static bool operator >(RenderState a, RenderState b)
        => a.Stage > b.Stage;
    public static bool operator <(RenderState a, RenderState b)
        => a.Stage < b.Stage;
    public static bool operator >=(RenderState a, RenderState b)
        => a.Stage >= b.Stage;
    public static bool operator <=(RenderState a, RenderState b)
        => a.Stage <= b.Stage;
    #endregion
    #region IComparable的接口实现
    public int CompareTo(RenderState? other)
        => Stage.CompareTo(other?.Stage ?? throw new ArgumentNullException(nameof(other)));
    #endregion
    #region 重写ToString
    public override string ToString() => Describe;
    #endregion
    #region 有关渲染阶段
    #region 渲染阶段
    /// <summary>
    /// 这个数字描述渲染的阶段，从1开始，
    /// 越大代表渲染的过程越完整
    /// </summary>
    public int Stage { get; init; }
    #endregion
    #region 对渲染阶段的描述
    /// <summary>
    /// 获取对渲染阶段的描述
    /// </summary>
    public string Describe { get; init; }
    #endregion
    #endregion
    #region 关于构造函数与创建对象
    #region 用于创建对象的静态方法
    #region OnInitializedAsync调用完毕后的渲染
    /// <summary>
    /// 返回一个渲染状态，
    /// 它表示此时OnInitializedAsync已经调用，
    /// 且返回未完成的<see cref="Task"/>，正在进行渲染，OnParametersSetAsync尚未调用
    /// </summary>
    public static RenderState CreateRender1 { get; }
        = new(1, $"OnInitializedAsync已经调用，且返回未完成的Task，执行渲染，OnParametersSetAsync尚未调用");
    #endregion
    #region OnParametersSetAsync调用完毕后的渲染
    /// <summary>
    /// 返回一个渲染状态，
    /// 它表示此时OnParametersSetAsync已经调用，
    /// 且返回未完成的<see cref="Task"/>，正在进行渲染，它不是最终的渲染
    /// </summary>
    public static RenderState CreateRender2 { get; }
        = new(2, "OnParametersSetAsync已经调用，且返回未完成的Task，执行渲染，它不是最终的渲染");
    #endregion
    #region 最终渲染
    /// <summary>
    /// 返回一个渲染状态，
    /// 它表示此时OnInitializedAsync和OnParametersSetAsync调用的<see cref="Task"/>都已经全部等待完毕，
    /// 这是最终的渲染，所有数据都已经被完全初始化
    /// </summary>
    public static RenderState CreateRender3 { get; }
        = new(3, "OnInitializedAsync和OnParametersSetAsync调用的Task都已经全部等待完毕，这是最终的渲染");
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的渲染阶段和描述初始化对象
    /// </summary>
    /// <param name="stage">这个数字描述渲染的阶段，越大代表渲染的过程越完整</param>
    /// <param name="describe">对渲染阶段的描述</param>
    public RenderState(int stage, string describe)
    {
        Stage = stage;
        Describe = describe;
    }
    #endregion
    #endregion
}
