namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以将对象填充到另一个对象中，
/// 它的一般用途是将API实体转换为数据库实体
/// </summary>
/// <typeparam name="Source">被填充对象的类型</typeparam>
/// <typeparam name="Fill">要填充的对象类型</typeparam>
public interface IFill<Source, in Fill> : IHasDeleteSideEffect<Source>
    where Source : class
{
    #region 创建或填充
    /// <summary>
    /// 创建或填充一个对象
    /// </summary>
    /// <param name="source">原始对象，
    /// 如果它为<see langword="null"/>，会被创建，否则会被填充</param>
    /// <param name="fill">用来填充的对象</param>
    /// <param name="dataPipe">用来获取数据的数据源，
    /// 如果数据之间存在关联关系，那么新数据需要从数据源中查询而来</param>
    /// <returns>一个元组，它的第一个项是创建或填充好的对象，
    /// 第二个项是假设新对象替代旧对象，所产生的副作用，
    /// 注意：按照规范，副作用应该在将更改推送到数据库之前调用</returns>
    abstract static (Source Object, Func<IServiceProvider, Task>? SideEffect) CreateOrFill(Source? source, Fill fill, IDataPipeFromContext dataPipe);
    #endregion
}
