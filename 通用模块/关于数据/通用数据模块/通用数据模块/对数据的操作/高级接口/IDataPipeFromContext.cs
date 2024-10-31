namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个查询上下文
/// </summary>
public interface IDataPipeFromContext
{
    #region 查询数据源抽象
    /// <summary>
    /// 返回一个数据源抽象，通过它可以查询数据源
    /// </summary>
    /// <typeparam name="Data">数据的类型</typeparam>
    /// <returns></returns>
    IQueryable<Data> Query<Data>()
        where Data : class;
    #endregion
    #region 获取所有受支持的实体类型
    /// <summary>
    /// 获取受这个查询上下文所支持的所有实体类型，
    /// 如果它为<see langword="null"/>，
    /// 表示支持任意类型，不受限制的实体类型
    /// </summary>
    IEnumerable<Type>? EntityTypes { get; }
    #endregion
}
