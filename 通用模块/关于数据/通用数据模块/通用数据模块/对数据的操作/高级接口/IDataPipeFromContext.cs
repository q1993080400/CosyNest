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
}
