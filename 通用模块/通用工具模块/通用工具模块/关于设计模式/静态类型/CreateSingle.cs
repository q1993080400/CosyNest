namespace System.Design;

/// <summary>
/// 这个静态类可以用来创建单例对象
/// </summary>
/// <typeparam name="Obj">单例对象的类型</typeparam>
public static class CreateSingle<Obj>
    where Obj : class, new()
{
    #region 创建单例对象
    /// <summary>
    /// 获取单例对象
    /// </summary>
    public static Obj Single { get; } = new();
    #endregion
}
