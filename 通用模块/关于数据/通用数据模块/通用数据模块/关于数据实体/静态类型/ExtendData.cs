using System.DataFrancis;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门声明有关数据实体的扩展方法

    #region 是否为新实体
    /// <summary>
    /// 判断一个实体是否为尚未保存的新实体
    /// </summary>
    /// <param name="data">要判断的实体</param>
    /// <returns></returns>
    public static bool IsNew(this IWithID data)
        => data.ID == default;
    #endregion
    #region 关于IWithID
    #region 筛选具有指定ID的实体
    #region 不可为null
    /// <summary>
    /// 筛选具有指定ID的实体
    /// </summary>
    /// <typeparam name="Entity">要筛选的实体的类型</typeparam>
    /// <param name="data">要筛选的数据源</param>
    /// <param name="id">要筛选的实体的ID</param>
    /// <returns></returns>
    public static Entity Find<Entity>(this IQueryable<Entity> data, Guid id)
        where Entity : IWithID
        => data.First(x => x.ID == id);
    #endregion
    #region 可能为null
    /// <summary>
    /// 筛选具有指定ID的实体，
    /// 如果不存在符合条件的实体，
    /// 则返回<see langword="null"/>
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Find{Entity}(IQueryable{Entity}, Guid)"/>
    public static Entity? FindOrDefault<Entity>(this IQueryable<Entity> data, Guid id)
        where Entity : IWithID
        => data.FirstOrDefault(x => x.ID == id);
    #endregion
    #endregion
    #region 判断是否存在具有指定ID的实体
    /// <summary>
    /// 判断是否存在具有指定ID的实体
    /// </summary>
    /// <typeparam name="Entity">要判断的实体的类型</typeparam>
    /// <param name="data">要判断的数据源</param>
    /// <param name="id">要判断的实体的ID</param>
    /// <returns></returns>
    public static bool Any<Entity>(this IQueryable<Entity> data, Guid id)
        where Entity : IWithID
        => data.Any(x => x.ID == id);
    #endregion
    #region 如果指定ID的实体不存在，则添加
    /// <summary>
    /// 如果数据库中不存在具有指定ID的实体，
    /// 则将它们添加进数据库
    /// </summary>
    /// <typeparam name="Entity">实体的类型</typeparam>
    /// <param name="pipe">数据管道对象</param>
    /// <param name="addEntity">要添加的实体，函数会检查它的ID</param>
    /// <returns></returns>
    public static async Task AddIfNotExist<Entity>(this IDataPipeToContext pipe, Entity addEntity)
        where Entity : class, IWithID
    {
        if (pipe.Query<Entity>().Any(addEntity.ID))
            return;
        await pipe.AddOrUpdate([addEntity], new()
        {
            IsAddData = AddOrUpdateInfo<Entity>.AddAllData
        });
    }
    #endregion
    #endregion
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
}
