using System.Reflection;
using System.Underlying;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="ConditionDynamicsRender{Condition}"/>的静态辅助类
/// </summary>
public static class ConditionDynamicsRender
{
    #region 根据屏幕类型匹配
    /// <summary>
    /// 这个高阶函数返回一个函数，
    /// 它传入用户当前的屏幕已经组件所在的程序集，
    /// 返回屏幕类型匹配当前屏幕，且前缀符合<paramref name="prefix"/>的组件
    /// </summary>
    /// <param name="prefix">要搜索的组件的前缀</param>
    /// <returns></returns>
    public static Func<IScreen, Assembly, Type> MatchScreenType(string prefix)
        => (screen, assembly) =>
        {
            var screenType = screen.ScreenType;
            var types = assembly.GetTypes().Where(x => x.Name.StartsWith(prefix)).ToArray();
            var match = types.SingleOrDefaultSecure(x => x.GetCustomAttributes<ConditionRenderMatchAttribute<ScreenType>>().Any(y => y.RenderCondition == screenType));
            return match ?? types.SingleOrDefaultSecure(x => x.HasAttributes<ConditionRenderAllAttribute<ScreenType>>()) ??
                throw new NotSupportedException($"在程序集{assembly}中没有找到标记了{nameof(ConditionRenderMatchAttribute<ScreenType>)}或{nameof(ConditionRenderAllAttribute<ScreenType>)}特性，" +
                $"且{nameof(ConditionRenderMatchAttribute<ScreenType?>.RenderCondition)}匹配null的组件，或找到了多个");
        };
    #endregion
}
