namespace System;

/// <summary>
/// 这个类型可以携带另一个对象，
/// 它经过初始化以后才可以使用
/// </summary>
/// <inheritdoc cref="ITag{Obj}"/>
/// <param name="initialization">用来初始化对象的委托</param>
public sealed class TagLazy<Obj>(Func<Task<Obj>> initialization) : ITag<Obj>
{
    #region 对象内容
    private Obj? ContentField;

    public Obj? Content
        => ContentField;
    #endregion
    #region 获取对象内容，不可为null
    public Obj CheckContent()
        => Content ?? throw new NullReferenceException($"{nameof(Content)}为null，无法获取这个对象");
    #endregion
    #region 初始化对象
    /// <summary>
    /// 初始化这个对象，使<see cref="Content"/>可以访问
    /// </summary>
    /// <returns></returns>
    public async Task Initialization()
    {
        ContentField ??= await initialization();
    }
    #endregion
}