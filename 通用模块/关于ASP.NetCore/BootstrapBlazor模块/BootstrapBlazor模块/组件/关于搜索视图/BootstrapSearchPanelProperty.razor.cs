using System.DataFrancis;

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
    #region 是否显示弹窗
    /// <summary>
    /// 获取是否显示弹窗
    /// </summary>
    private bool IsShow { get; set; }
    #endregion
    #region 提交枚举搜索
    /// <summary>
    /// 这个高阶函数返回一个函数，
    /// 它可以用来绑定要筛选的枚举
    /// </summary>
    /// <param name="bind">要绑定的对象</param>
    /// <returns></returns>
    private Func<IReadOnlyCollection<EnumItem>, Task> SubmitEnumItem(IBindProperty<string> bind)
        => async x =>
        {
            bind.Value = x.Single().Value;
            await RenderPropertyInfo.Submit();
            IsShow = false;
        };
    #endregion
    #region 清除枚举搜索
    /// <summary>
    /// 这个高阶函数返回一个函数，
    /// 它可以用来清除要筛选的枚举
    /// </summary>
    /// <param name="bind">要绑定的对象</param>
    /// <returns></returns>
    private Func<Task> ClearEnumItem(IBindProperty<string> bind)
        => async () =>
        {
            bind.Value = null;
            await RenderPropertyInfo.Submit();
            IsShow = false;
        };
    #endregion
    #region 捕获的选择器对象
    /// <summary>
    /// 获取捕获的选择器对象
    /// </summary>
    private Selector<EnumItem>? Selector { get; set; }
    #endregion
    #endregion
}
