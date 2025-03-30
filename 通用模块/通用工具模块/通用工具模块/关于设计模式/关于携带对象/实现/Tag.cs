namespace System.Design;

/// <summary>
/// 这个类型可以携带另一个对象，
/// 它通常用于级联参数或类似的场合
/// </summary>
/// <inheritdoc cref="ITag{Obj}"/>
public sealed record class Tag<Obj> : ITag<Obj>
{
    #region 对象内容
    public Obj? Content { get; set; }
    #endregion
    #region 获取对象内容，不可为null
    public Obj CheckContent()
        => Content ?? throw new NullReferenceException($"{nameof(Content)}为null，无法获取这个对象");
    #endregion
}