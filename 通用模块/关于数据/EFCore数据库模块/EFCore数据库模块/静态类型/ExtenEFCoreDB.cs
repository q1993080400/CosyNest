﻿using System.Collections;
using System.DataFrancis;
using System.DataFrancis.DB.EF;
using System.Performance;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace System;

/// <summary>
/// 有关EF数据库的扩展方法全部放在这里
/// </summary>
public static class ExtenEFCoreDB
{
    #region 启用级联删除
    #region 指定实体
    /// <summary>
    /// 级联删除指定的实体，
    /// 它要求实体使用立即加载或惰性加载依赖的实体，
    /// 如果依赖的实体被忽略，则级联删除失效
    /// </summary>
    /// <typeparam name="Data">实体的类型</typeparam>
    /// <param name="dbSet">实体所在的数据表</param>
    /// <param name="datas">要级联删除的实体</param>
    internal static void CascadingDelete<Data>(this DbSet<Data> dbSet, IEnumerable<Data> datas)
        where Data : class
    {
        #region 本地函数
        static void Fun(object? obj, IEntityType entityType)
        {
            if (obj is null)
                return;
            foreach (var navigation in NavigationsCache[entityType])
            {
                var propertie = navigation.PropertyInfo!;
                if (propertie!.GetValue(obj) is not IEnumerable propertieValue)
                    continue;
                foreach (var item in propertieValue)
                {
                    Fun(item, navigation.TargetEntityType);
                }
            }
        }
        #endregion
        foreach (var item in datas)
        {
            Fun(item, dbSet.EntityType);
        }
        dbSet.RemoveRange(datas);
    }
    #endregion
    #region 指定表达式

    #endregion
    #region 辅助方法：缓存所有依赖实体
    /// <summary>
    /// 这个缓存的键是实体的元数据，
    /// 值是该实体的所有依赖实体导航属性
    /// </summary>
    private static ICache<IEntityType, IEnumerable<INavigation>> NavigationsCache { get; }
    = CreatePerformance.CacheThreshold(entityType =>
    {
        var collectionType = typeof(ICollection<>);
        return entityType.GetNavigations().Where(x =>
        {
            var property = x.PropertyInfo ??
            throw new NullReferenceException($"实体类型{x.DeclaringEntityType.Name}的导航属性{x.Name}是一个字段，请使用属性");
            var type = property.PropertyType;
            return type.IsGenericRealize(collectionType) &&
                !collectionType.MakeGenericType(property.GetBaseDefinition().DeclaringType!).IsAssignableFrom(type);
        }).ToArray();
    }, 150, NavigationsCache);

    /*说明文档
      排除property.GetBaseDefinition().DeclaringType，
      主要是为了防止多对多关系中出现的级联删除错误，
      假设A和B是多对多关系，而且没有这句话，那么A和B会无限递归删除*/
    #endregion
    #endregion
    #region 返回数据库中是否存在指定实体
    /// <summary>
    /// 返回数据库中是否存在指定的实体
    /// </summary>
    /// <param name="db">要查找实体的数据库</param>
    /// <param name="entityType">要检测的实体类型</param>
    /// <returns></returns>
    public static bool ContainsEntityType(this DbContext db, Type entityType)
        => db.Model.FindEntityType(entityType) is { };
    #endregion
    #region 添加支持表连接的数据管道
    /// <summary>
    /// 向服务容器以瞬间模式添加一个支持表连接的数据管道
    /// </summary>
    /// <typeparam name="DB">数据库上下文的类型</typeparam>
    /// <param name="services">要添加的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddDataPipeDBWithJoin<DB>(this IServiceCollection services)
        where DB : DbContextFrancis, new()
        => services.AddTransient(_ =>
        {
            var db = new DB();
            return (IDataPipeDBWithJoin)CreateEFCoreDB.Pipe(_ => db);
        });
    #endregion
}
