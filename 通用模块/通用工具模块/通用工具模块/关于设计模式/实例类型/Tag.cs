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
    #region 获取对象内容，不可为null
    /// <summary>
    /// 获取对象封装的内容，
    /// 如果为<see langword="null"/>，
    /// 会引发一个异常
    /// </summary>
    /// <returns></returns>
    public Obj CheckContent()
        => Content ?? throw new NullReferenceException($"{nameof(Content)}为null，无法获取这个对象");
    #endregion
}
