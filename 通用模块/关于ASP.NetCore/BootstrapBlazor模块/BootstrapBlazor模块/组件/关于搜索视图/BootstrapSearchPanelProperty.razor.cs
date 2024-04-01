using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件可以用来渲染<see cref="BootstrapSearchPanel{BusinessInterface}"/>中的某一个属性
/// </summary>
public sealed partial class BootstrapSearchPanelProperty : ComponentBase
{
    #region 组件参数
    #region 属性的渲染参数
    /// <summary>
    /// 获取属性的渲染参数
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderSearchPanelPropertyInfo RenderPropertyInfo { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 获取绑定的枚举
    /// <summary>
    /// 获取绑定的枚举缓存
    /// </summary>
    /// <param name="query">渲染查询条件</param>
    /// <returns></returns>
    private BindQueryCondition<string?> GetBindEnum(RenderQueryCondition query)
        => RenderPropertyInfo.SearchViewerState.Bind<string?>(query);
    #endregion
    #region 当选择的枚举被改变时触发的委托
    /// <summary>
    /// 当选择的枚举被改变时触发的委托
    /// </summary>
    /// <param name="enum">枚举的新值</param>
    /// <param name="query">查询条件</param>
    /// <returns></returns>
    private async Task OnEnumChanged(string? @enum, RenderQueryCondition query)
    {
        var bind = GetBindEnum(query);
        bind.Value = @enum.IsVoid() ? null : @enum?.ToString();
        await RenderPropertyInfo.Submit();
    }
    #endregion
    #endregion
}
