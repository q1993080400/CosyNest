using System.Collections;
using System.Performance;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace System.DataFrancis.DB;

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
        where Data : class, IData
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
}
