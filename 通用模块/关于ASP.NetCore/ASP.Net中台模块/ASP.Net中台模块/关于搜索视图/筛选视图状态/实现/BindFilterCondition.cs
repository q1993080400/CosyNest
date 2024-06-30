using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型是所有筛选条件绑定的基类
/// </summary>
/// <param name="renderFilter">封装的筛选条件</param>
/// <typeparam name="Target">筛选目标的类型</typeparam>
/// <typeparam name="Action">筛选动作的类型</typeparam>
/// <typeparam name="Contain">包含对象的类型</typeparam>
abstract class BindFilterCondition<Target, Action, Contain>(RenderFilter<Target, Action> renderFilter) : IGenerateFilter, IBind<Contain>
   where Target : FilterTarget
   where Action : FilterAction
{
    #region 公开成员
    #region 生成筛选条件
    public abstract DataCondition[] GenerateFilter();
    #endregion
    #region 筛选标识
    public string Identification
        => RenderFilter.FilterTarget.Identification;
    #endregion
    #endregion
    #region 内部成员
    #region 封装的筛选条件
    /// <summary>
    /// 获取封装的筛选条件
    /// </summary>
    protected RenderFilter<Target, Action> RenderFilter { get; } = renderFilter;
    #endregion
    #region Trim筛选值文本
    /// <summary>
    /// 如果筛选值是文本，则Trim它，
    /// 否则原路返回它
    /// </summary>
    /// <param name="value">要处理的筛选值</param>
    /// <returns></returns>
    protected static object? TrimValue(object? value)
        => value switch
        {
            string text => text.Trim() switch
            {
                "" => null,
                var t => t
            },
            _ => value
        };
    #endregion
    #endregion
}
