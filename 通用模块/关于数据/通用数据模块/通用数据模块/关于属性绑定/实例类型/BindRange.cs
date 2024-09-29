namespace System.DataFrancis;

/// <summary>
/// 这个类型是一个用于绑定的范围，
/// 它具有一个开始和结束
/// </summary>
/// <typeparam name="Obj">筛选条件的类型</typeparam>
public sealed class BindRange<Obj>
{
    #region 范围的开始
    /// <summary>
    /// 获取范围的开始
    /// </summary>
    public Obj? Start { get; set; }
    #endregion
    #region 范围的结束
    /// <summary>
    /// 获取范围的结束
    /// </summary>
    public Obj? End { get; set; }
    #endregion
}
