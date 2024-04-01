using System.Reflection;

namespace Microsoft.AspNetCore;

/// <summary>
/// 有关搜索视图的工具类
/// </summary>
public static class ToolSearchViewer
{
    #region 根据实体类获取筛选条件
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
        var type = typeof(Entity);
        var typeAttribute = type.GetCustomAttributes<FilterConditionAttribute<BusinessInterface>>().
            Select(x => x.ConvertConditioGroup(type));
        var almightyPropertys = type.GetTypeData().AlmightyPropertys;
        var ppropertyAttribute = almightyPropertys.Select(x =>
        {
            var attributes = x.GetCustomAttributes<FilterConditionAttribute<BusinessInterface>>();
            return attributes.Select(y => y.ConvertConditioGroup(x));
        }).SelectMany(x => x);
        return [.. typeAttribute.Concat(ppropertyAttribute).ToArray().OrderBy(x => x.Order)];
    }
    #endregion
}
