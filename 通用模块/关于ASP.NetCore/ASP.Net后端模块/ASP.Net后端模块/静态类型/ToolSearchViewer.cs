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
    /// <typeparam name="Entity">实体类的类型</typeparam>
    /// <typeparam name="BusinessInterface">业务接口的类型，
    /// 只筛选属于这个业务接口的筛选条件</typeparam>
    /// <returns></returns>
    public static RenderConditionGroup[] GetRenderCondition<Entity, BusinessInterface>()
        where BusinessInterface : class, IGetRenderAllFilterCondition
    {
        var entityType = typeof(Entity);
        var interfaceType = typeof(BusinessInterface);
        var key = (entityType, interfaceType);
        if (CacheRenderCondition.TryGetValue(key, out var renderConditionGroup))
            return renderConditionGroup ?? throw new NullReferenceException("获取到的缓存为null");
        var typeAttribute = entityType.GetCustomAttributes<FilterConditionAttribute<BusinessInterface>>().
            Select(x => x.ConvertConditioGroup(entityType));
        var almightyPropertys = entityType.GetTypeData().AlmightyPropertys;
        var propertyAttribute = almightyPropertys.Select(x =>
        {
            var attributes = x.GetCustomAttributes<FilterConditionAttribute<BusinessInterface>>();
            return attributes.Select(y => y.ConvertConditioGroup(x));
        }).SelectMany(x => x);
        RenderConditionGroup[] array = [.. typeAttribute.Concat(propertyAttribute).ToArray().OrderBy(x => x.Order)];
        CacheRenderCondition.SetValue(key, array);
        return array;
    }
    #endregion
    #region 缓存属性
    /// <summary>
    /// 缓存渲染筛选条件
    /// </summary>
    private static ICache<(Type Entity, Type BusinessInterface), RenderConditionGroup[]> CacheRenderCondition { get; }
    = CreatePerformance.MemoryCache<(Type Entity, Type BusinessInterface), RenderConditionGroup[]>
        ((_, _) => throw new NotSupportedException("不支持自动获取元素，请显式添加元素，然后获取"));
    #endregion
    #endregion
}
