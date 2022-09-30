namespace System.Realize;

/// <summary>
/// 该类型是<see cref="IComparer{T}"/>的快速实现，
/// 它通过委托进行比较
/// </summary>
/// <typeparam name="Obj">要比较的对象类型</typeparam>
sealed class Comparer<Obj> : IComparer<Obj>
{
    #region 封装的委托
    /// <summary>
    /// 用于比较两个对象的委托
    /// </summary>
    private Func<Obj?, Obj?, int> IsGreater { get; }
    #endregion
    #region 接口实现
    public int Compare(Obj? x, Obj? y)
        => IsGreater(x, y);
    #endregion
    #region 构造函数
    /// <inheritdoc cref="FastRealize.Comparer{Obj}(Func{Obj, Obj, int})"/>
    public Comparer(Func<Obj?, Obj?, int> isGreater)
    {
        this.IsGreater = isGreater;
    }
    #endregion
}
