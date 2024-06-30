namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以将对象填充到另一个对象中
/// </summary>
/// <typeparam name="Obj">原始对象的类型</typeparam>
/// <typeparam name="Fill">要填充的对象类型</typeparam>
public interface IFill<Obj, Fill>
    where Obj : class
{
    #region 创建或填充
    /// <summary>
    /// 创建或填充一个对象
    /// </summary>
    /// <param name="source">原始对象，
    /// 如果它为<see langword="null"/>，会被创建，否则会被填充</param>
    /// <param name="fill">用来填充的对象</param>
    /// <returns></returns>
    abstract static Obj CreateOrFill(Obj? source, Fill fill);
    #endregion
}
