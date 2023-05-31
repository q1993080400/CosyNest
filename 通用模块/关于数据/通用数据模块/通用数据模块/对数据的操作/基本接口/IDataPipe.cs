using System.Performance;

namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个双向的数据管道，
/// 它既可以拉取又可以推送数据
/// </summary>
public interface IDataPipe : IDataPipeTo, IDataPipeFrom
{
    #region 静态方法
    #region 创建数据对象
    #region 缓存对象
    /// <summary>
    /// 该缓存的键是数据的类型，
    /// 值是一个委托，通过它可以创建新的数据
    /// </summary>
    private static ICache<Type, Func<IData>> CreateDataCache { get; }
    = CreatePerformance.CacheThreshold(type =>
    {
        if (type == typeof(IData))
            return static () => CreateDataObj.DataEmpty();
        var c = type.GetConstructor(Array.Empty<Type>()) ??
            throw new NotSupportedException($"{type}没有公开无参数构造函数，无法创建它的实例");
        return () => (IData)c.Invoke(Array.Empty<object>());
    }, 100, CreateDataCache);
    #endregion
    #region 正式方法
    /// <summary>
    /// 返回一个委托，它可以用来创建指定类型的空数据，
    /// 但是前提是该类型拥有公开无参数构造函数，或者等同于<see cref="IData"/>
    /// </summary>
    /// <typeparam name="Data">要创建的数据的类型</typeparam>
    /// <returns></returns>
    public static Func<Data> CreateData<Data>()
        where Data : IData
    {
        var cache = CreateDataCache[typeof(Data)];
        return () => (Data)cache();
    }
    #endregion
    #endregion
    #endregion
    #region 执行事务
    #region 无返回值
    /// <summary>
    /// 执行一个事务，如果事务出现问题，
    /// 则它做出的改动全部撤销
    /// </summary>
    /// <param name="transaction">要执行的事务委托，
    /// 它可以访问一个<see cref="IDataPipe"/>对象，只要将这个对象一直传递下去，
    /// 就可以保证事务一致性</param>
    /// <returns></returns>
    Task Transaction(Func<IDataPipe, Task> transaction);
    #endregion
    #region 有返回值
    /// <summary>
    /// 执行一个事务，并返回它的返回值，如果事务出现问题，
    /// 则它做出的改动全部撤销
    /// </summary>
    /// <typeparam name="Obj">事务返回值的类型</typeparam>
    /// <inheritdoc cref="Transaction(Func{IDataPipe, Task})"/>
    Task<Obj> Transaction<Obj>(Func<IDataPipe, Task<Obj>> transaction);
    #endregion
    #endregion
}
