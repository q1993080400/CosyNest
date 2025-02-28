using System.DataFrancis;
using System.Reflection;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门声明有关具有日期的实体的扩展方法

    #region 关于具有过期时间的实体
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
    #region 搜索所有过期的实体，并删除它们
    #region 正式方法
    /// <summary>
    /// 搜索所有实现了<see cref="IWithLife{Entity}"/>，
    /// 且已经过期的实体，并删除它们，
    /// 如果它们实现了<see cref="IHasDeleteSideEffect{Entity}"/>，还会执行副作用
    /// </summary>
    /// <param name="pipe">数据管道</param>
    /// <param name="serviceProvider">一个用于请求服务的对象</param>
    /// <returns></returns>
    public static async Task DeleteAllExpire(this IDataPipe pipe, IServiceProvider serviceProvider)
    {
        var entityTypes = pipe.EntityTypes ??
            throw new NotSupportedException($"无法获取{pipe.GetType()}的所有实体类");
        var withLifeGenericParameters = entityTypes.Select(type =>
        {
            if (type.IsAbstract || !type.IsClass)
                return (Type[]?)null;
            var (isRealize, _, genericParameter) = type.IsRealizeGeneric(typeof(IWithLife<>));
            return isRealize ? [genericParameter[0], type] : null;
        }).WhereNotNull().ToArray();
        var methodInfo = typeof(ExtendData).GetMethod(nameof(DeleteAllExpireRealize),
            BindingFlags.Static | BindingFlags.NonPublic,
            [typeof(IDataPipe), typeof(IServiceProvider)]) ??
            throw new NotSupportedException($"没有找到名叫{nameof(DeleteAllExpireRealize)}，符合标准的方法");
        foreach (var genericParameter in withLifeGenericParameters)
        {
            var makeMethodInfo = methodInfo.MakeGenericMethod(genericParameter);
            await makeMethodInfo.Invoke<Task>(null, pipe, serviceProvider);
        }
    }
    #endregion
    #region 辅助方法
    /// <summary>
    /// 搜索过期的实体，
    /// 如果这个实体实现了<see cref="IHasDeleteSideEffect{Entity}"/>，
    /// 还会执行执行副作用
    /// </summary>
    /// <typeparam name="Data">实现接口的实体类型</typeparam>
    /// <typeparam name="DerivativeEntity">实体的派生实体，
    /// 它可以用于为派生类生成表达式</typeparam>
    /// <param name="pipe">数据管道</param>
    /// <param name="serviceProvider">一个用于请求服务的对象</param>
    /// <returns></returns>
    private static async Task DeleteAllExpireRealize<Data, DerivativeEntity>(IDataPipe pipe, IServiceProvider serviceProvider)
        where Data : class, IWithLife<Data>
        where DerivativeEntity : class, Data
    {
        var datas = pipe.Query<DerivativeEntity>().WhereLife<Data, DerivativeEntity>(true);
        await datas.ExecuteDeleteAndClean(pipe, serviceProvider);
    }
    #endregion
    #endregion
    #endregion
    #region 关于具有更新时间的实体
    #region 筛选状态发生改变的实体
    /// <summary>
    /// 筛选在指定的时间后，状态发生改变的实体
    /// </summary>
    /// <typeparam name="Entity">要检查的实体的类型</typeparam>
    /// <param name="entities">要检查的实体</param>
    /// <param name="lastDate">指定的时间</param>
    /// <returns></returns>
    public static IQueryable<Entity> WhereUpdate<Entity>(this IQueryable<Entity> entities, DateTimeOffset lastDate)
        where Entity : class, IWithUpdateDate
        => entities.Where(x => x.UpdateDate > lastDate);
    #endregion
    #region 判断状态是否发生改变
    /// <summary>
    /// 判断在指定的时间之后，
    /// 是否存在状态已经发生改变的实体
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="WhereUpdate{Entity}(IQueryable{Entity}, DateTimeOffset)"/>
    public static bool AnyUpdate<Entity>(this IQueryable<Entity> entities, DateTimeOffset lastDate)
        where Entity : class, IWithUpdateDate
        => entities.WhereUpdate(lastDate).Any();
    #endregion
    #endregion
}
