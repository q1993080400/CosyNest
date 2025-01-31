using System.DataFrancis;

using Microsoft.EntityFrameworkCore;

namespace System;

/// <summary>
/// 有关EF数据库的扩展方法全部放在这里
/// </summary>
public static partial class ExtendEFCoreDB
{
    #region 删除已经过期的实体
    #region 可指定派生类
    /// <summary>
    /// 删除所有已经过期的实体
    /// </summary>
    /// <typeparam name="Data">实现接口的实体类型</typeparam>
    /// <typeparam name="DerivativeEntity">实体的派生实体，
    /// 它可以用于为派生类生成表达式</typeparam>
    /// <param name="entities">要删除的实体数据源</param>
    /// <returns></returns>
    public static Task ExecuteDeleteExpire<Data, DerivativeEntity>(this IQueryable<DerivativeEntity> entities)
        where Data : class, IWithLife<Data>
        where DerivativeEntity : Data
        => entities.WhereLife<Data, DerivativeEntity>(true).ExecuteDeleteAsync();
    #endregion
    #region 直接删除当前类型
    /// <inheritdoc cref="ExecuteDeleteExpire{Data, DerivativeEntity}(IQueryable{DerivativeEntity})"/>
    public static Task ExecuteDeleteExpire<Data>(this IQueryable<Data> entities)
        where Data : class, IWithLife<Data>
    {
        var type = typeof(Data);
        if (type.IsAbstract)
            throw new NotSupportedException($"{type.Name}是一个抽象的实体类，为避免引起意外的后果，不允许直接执行这个删除操作，" +
                $"请显式使用本方法具有两个泛型参数的重载");
        return entities.ExecuteDeleteExpire<Data, Data>();
    }
    #endregion
    #endregion
}
