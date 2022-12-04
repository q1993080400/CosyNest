using System.Linq.Expressions;
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
    #region 按条件删除数据
    #region 说明文档
    /*问：按照通常的思维习惯，删除数据和添加数据是相对应的，
      这个功能应该放在IDataPipeTo中，那么为什么会放在这里？
      答：因为这个API的功能是“删除符合指定条件的数据”，
      因此它需要先进行一次查询操作，不能查询数据的IDataPipeTo不适合加入这个API

      问：既然如此，如果要支持删除数据，是否必须实现IDataPipe接口？
      答：不是，即便没有实现IDataPipe接口，仍然可以通过IDataPipeTo.Delete方法来删除数据，
      但是这种方法必须先获取数据，后删除，性能较低*/
    #endregion
    #region 正式方法
    /// <summary>
    /// 直接从数据源中删除符合指定谓词的数据，不返回结果集
    /// </summary>
    /// <param name="expression">一个用来指定删除条件的表达式</param>
    /// <inheritdoc cref="IDataPipeFrom.Query{Data}"/>
    Task Delete<Data>(Expression<Func<Data, bool>> expression, CancellationToken cancellation = default)
        where Data : class, IData;
    #endregion
    #endregion
}
