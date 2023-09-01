namespace System.DataFrancis;

/// <summary>
/// 这个类型是一个最简单的<see cref="IDataPipeFrom"/>实现，
/// 它通过<see cref="IQueryable{T}"/>工厂来获取数据
/// </summary>
/// <typeparam name="Entity">数据的类型</typeparam>
/// <remarks>
/// 使用指定的查询表达式初始化对象
/// </remarks>
/// <param name="factory">用来创建查询表达式的工厂</param>
sealed class DataPipeFromFactory<Entity>(Func<IQueryable<Entity>> factory) : IDataPipeFrom
{
    #region 公开成员
    IQueryable<Data> IDataPipeFrom.Query<Data>()
        => Factory().To<IQueryable<Data>>();
    #endregion
    #region 内部成员
    /// <summary>
    /// 这个工厂被用来创建查询表达式
    /// </summary>
    private Func<IQueryable<Entity>> Factory { get; } = factory;

    #endregion
}
