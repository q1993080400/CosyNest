using System.Performance;
using System.Reflection;

namespace Microsoft.AspNetCore;

/// <summary>
/// 有关搜索视图的工具类
/// </summary>
public static class ToolSearchViewer
{
    #region 根据实体类获取筛选条件
    #region 正式方法
    /// <summary>
    /// 根据实体类获取筛选条件
    /// </summary>
    /// <typeparam name="DBEntity">数据库实体类的类型</typeparam>
    /// <typeparam name="BusinessInterface">业务接口的类型，
    /// 只筛选属于这个业务接口的筛选条件</typeparam>
    /// <returns></returns>
    public static RenderFilterGroup[] GetRenderCondition<DBEntity, BusinessInterface>()
        where BusinessInterface : class, IGetRenderAllFilterCondition
    {
        var interfaceType = typeof(BusinessInterface);
        if (CacheRenderCondition.TryGetValue(interfaceType, out var renderConditionGroup))
            return renderConditionGroup;
        var entityType = typeof(DBEntity);
        var typeAttribute = entityType.GetCustomAttributes<FilterConditionAttribute<BusinessInterface>>().
            Select(x => x.ConvertConditioGroup(entityType));
        var almightyPropertys = entityType.GetPropertyInfoAlmighty();
        var propertyAttribute = almightyPropertys.Select(x =>
        {
            var attributes = x.GetCustomAttributes<FilterConditionAttribute<BusinessInterface>>();
            return attributes.Select(y => y.ConvertConditioGroup(x));
        }).SelectMany(x => x);
        RenderFilterGroup[] array = [.. typeAttribute.Concat(propertyAttribute).ToArray().OrderBy(x => x.Order)];
        CacheRenderCondition.SetValue(interfaceType, array);
        return array;
    }
    #endregion
    #region 缓存属性
    /// <summary>
    /// 这个属性是一个缓存，
    /// 它按照业务接口的类型索引渲染条件
    /// </summary>
    private static ICache<Type, RenderFilterGroup[]> CacheRenderCondition { get; }
    = CreatePerformance.MemoryCache<Type, RenderFilterGroup[]>
        (static _ => throw new NotSupportedException("不支持自动获取元素，请显式添加元素，然后获取"));
    #endregion
    #endregion
}
