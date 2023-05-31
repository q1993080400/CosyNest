namespace System.Design;

/// <summary>
/// 这个类型可以携带另一个对象，
/// 它通常用于级联参数或类似的场合
/// </summary>
/// <typeparam name="Obj">携带的对象的类型</typeparam>
public sealed record class Tag<Obj>
{
    #region 对象内容
    /// <summary>
    /// 获取这个对象封装的内容
    /// </summary>
    public Obj? Content { get; set; }
    #endregion
}
