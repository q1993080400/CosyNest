using System.DataFrancis.EntityDescribe;
using System.DataFrancis;
using System.Reflection;

using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本组件是<see cref="FormViewer{Model}"/>的开箱即用版，
/// 它底层由Bootstrap实现
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed partial class BootstrapFormViewer<Model> : ComponentBase, IContentComponent<RenderFragment<BootstrapRenderFormViewerInfo<Model>>>
    where Model : class, new()
{
    #region 组件参数
    #region 用来渲染每个属性的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderProperty"/>
    [Parameter]
    public RenderFragment<RenderFormViewerPropertyInfo<Model>>? RenderProperty { get; set; }
    #endregion
    #region 用来渲染组件的委托
    [Parameter]
    public RenderFragment<BootstrapRenderFormViewerInfo<Model>>? ChildContent { get; set; }
    #endregion
    #region 用来渲染主体部分的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderMain"/>
    [Parameter]
    public RenderFragment<IEnumerable<RenderFormViewerMainInfo>>? RenderMain { get; set; }
    #endregion
    #region 用来初始化模型的委托
    /// <inheritdoc cref="FormViewer{Model}.InitializationModel"/>
    [Parameter]
    public Func<Task<Model?>> InitializationModel { get; set; }
        = () => Task.FromResult<Model?>(null);
    #endregion
    #region 用来重置模型的委托
    /// <inheritdoc cref="FormViewer{Model}.ResetModel"/>
    [Parameter]
    public Func<Model, Model> ResetModel { get; set; } = _ => new();
    #endregion
    #region 用来判断是否为现有表单的委托
    /// <inheritdoc cref="FormViewer{Model}.ExistingForms"/>
    [Parameter]
    public Func<Model, bool> ExistingForms { get; set; } = _ => false;
    #endregion
    #region 用来筛选属性的委托
    /// <inheritdoc cref="FormViewer{Model}.FilterProperties"/>
    [Parameter]
    public Func<PropertyInfo, bool> FilterProperties { get; set; } = FormViewer<Modal>.FilterPropertiesDefault;
    #endregion
    #region 用来将属性转换为渲染参数的委托
    /// <inheritdoc cref="FormViewer{Model}.ToPropertyInfo"/>
    [Parameter]
    public Func<IEnumerable<PropertyInfo>, Model, IEnumerable<RenderFormViewerPropertyInfo<Model>>> ToPropertyInfo { get; set; } = FormViewer<Model>.ToPropertyInfoDefault;
    #endregion
    #region 用来验证数据的委托
    /// <inheritdoc cref="FormViewer{Model}.Verify"/>
    [Parameter]
    public DataVerify Verify { get; set; } = CreateDataObj.DataVerifyDefault;
    #endregion
    #region 用来提交表单的业务逻辑
    /// <summary>
    /// 获取或设置用于提交表单的业务逻辑，
    /// 它的参数就是当前表单的模型以及验证结果，
    /// 返回值是业务逻辑是否成功
    /// </summary>
    [Parameter]
    public Func<VerificationResults, Task<bool>> Submit { get; set; } = _ => Task.FromResult(true);
    #endregion
    #region 用来重置表单的业务逻辑
    /// <summary>
    /// 获取或设置用于重置表单的业务逻辑，
    /// 当执行这个委托的时候，模型已被恢复为初始状态
    /// </summary>
    [Parameter]
    public Func<Task> Resetting { get; set; } = () => Task.CompletedTask;
    #endregion
    #region 刷新目标
    /// <summary>
    /// 获取刷新目标，
    /// 它决定了在提交，重置，删除表单时，应该刷新哪个组件
    /// </summary>
    [Parameter]
    public IHandleEvent RefreshTarget { get; set; }
    #endregion
    #region 用来删除表单的业务逻辑
    /// <summary>
    /// 获取或设置用于删除表单的业务逻辑，
    /// 它的参数就是当前表单的模型以及验证结果，
    /// 返回值是业务逻辑是否成功
    /// </summary>
    [Parameter]
    public Func<VerificationResults, Task<bool>> Delete { get; set; } = _ => Task.FromResult(true);
    #endregion
    #endregion
    #region 内部成员
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <param name="info">基础版本的渲染参数</param>
    /// <returns></returns>
    private BootstrapRenderFormViewerInfo<Model> GetRenderInfo(RenderFormViewerInfo<Model> info)
        => new()
        {
            RenderFormViewerInfo = info,
            Delete = new(RefreshTarget, async () =>
            {
                var data = info.ModelAndVerify();
                var isSuccess = await Delete(data);
                if (isSuccess)
                    info.Resetting();
            }),
            Resetting = new(RefreshTarget, async () =>
            {
                info.Resetting();
                await Resetting();
            }),
            Submit = new(RefreshTarget, async () =>
            {
                var data = info.ModelAndVerify();
                var isSuccess = await Submit(data);
                if (isSuccess)
                    info.Resetting();
            })
        };
    #endregion
    #endregion
    #region 构造函数
    public BootstrapFormViewer()
    {
        RefreshTarget = this;
    }
    #endregion
}
