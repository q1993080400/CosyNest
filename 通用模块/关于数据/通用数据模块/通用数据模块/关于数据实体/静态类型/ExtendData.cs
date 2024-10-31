using System.DataFrancis;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门声明有关数据实体的扩展方法

    #region 筛选已经过期或未过期的实体
    #region 可指定派生类
    /// <summary>
    /// 筛选已过期或未过期的实体
    /// </summary>
    /// <typeparam name="Data">实现接口的实体类型</typeparam>
    /// <typeparam name="DerivativeEntity">实体的派生实体，
    /// 它可以用于为派生类生成表达式</typeparam>
    /// <param name="data">数据源</param>
    /// <param name="filterExpire">如果这个值为<see langword="true"/>，
    /// 则筛选已经过期的实体，否则筛选未过期的实体</param>
    /// <returns></returns>
    public static IQueryable<DerivativeEntity> WhereLife<Data, DerivativeEntity>(this IQueryable<DerivativeEntity> data, bool filterExpire)
        where Data : class, IWithLife<Data>
        where DerivativeEntity : Data
        => data.Where(Data.GetFilterExpression<DerivativeEntity>(filterExpire));
    #endregion
    #region 直接查询当前类型
    /// <inheritdoc cref="WhereLife{Data, DerivativeEntity}(IQueryable{DerivativeEntity}, bool)"/>
    public static IQueryable<Data> WhereLife<Data>(this IQueryable<Data> data, bool filterExpire)
        where Data : class, IWithLife<Data>
    {
        var type = typeof(Data);
        if (type.IsAbstract)
            throw new NotSupportedException($"{type.Name}是一个抽象的实体类，为避免引起意外的后果，不允许直接执行这个查询，" +
                $"请显式使用本方法具有两个泛型参数的重载");
        return data.WhereLife<Data, Data>(filterExpire);
    }
    #endregion 
    #endregion
    #region 枚举实体，并执行它们的收尾操作
    /// <summary>
    /// 枚举可清理实体，并对它们执行收尾操作，
    /// 然后删除这些实体
    /// </summary>
    /// <typeparam name="Clean">可清理实体的类型</typeparam>
    /// <param name="entitys">要执行收尾和删除操作的实体</param>
    /// <param name="dataPipe">用来执行删除操作的数据上下文</param>
    /// <param name="serviceProvider">一个用来请求服务的对象，
    /// 根据实现的不同，这个函数可能不需要它</param>
    /// <returns></returns>
    public static async Task ExecuteDeleteAndClean<Clean>(this IQueryable<Clean> entitys, IDataPipeToContext dataPipe, IServiceProvider? serviceProvider = null)
        where Clean : class, IClean<Clean>
    {
        var array = entitys.ToArray();
        await Clean.Clean(array, serviceProvider);
        await dataPipe.Delete(array);
    }
    #endregion
}
