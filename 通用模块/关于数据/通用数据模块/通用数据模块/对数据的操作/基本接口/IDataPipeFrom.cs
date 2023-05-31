namespace System.DataFrancis;

/// <summary>
/// 所有实现这个接口的类型，
/// 都可以作为一个支持从数据源获取数据的管道
/// </summary>
public interface IDataPipeFrom
{
    #region 查询数据源抽象
    /// <summary>
    /// 返回一个数据源抽象，通过它可以查询数据源
    /// </summary>
    /// <typeparam name="Data">数据的类型</typeparam>
    /// <returns></returns>
    IQueryable<Data> Query<Data>()
        where Data : class, IData;
    #endregion
}
