using System.DataFrancis.EntityDescribe;
using System.Reflection;

using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本组件是<see cref="FormViewer{Model}"/>的开箱即用版，
/// 它底层由Bootstrap实现
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed partial class BootstrapFormViewer<Model> : ComponentBase, IContentComponent<RenderFragment<RenderFormViewerInfo<Model>>>
    where Model : class, new()
{
    #region 组件参数
    #region 关于渲染
    #region 用来渲染每个属性的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderProperty"/>
    [Parameter]
    public RenderFragment<RenderFormViewerPropertyInfo<Model>>? RenderProperty { get; set; }
    #endregion
    #region 用来渲染组件的委托
    [Parameter]
    public RenderFragment<RenderFormViewerInfo<Model>>? ChildContent { get; set; }
    #endregion
    #region 用来渲染主体部分的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderMain"/>
    [Parameter]
    public RenderFragment<IEnumerable<RenderFormViewerMainInfo<Model>>>? RenderMain { get; set; }
    #endregion
    #region 用来渲染提交部分的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderSubmit"/>
    [Parameter]
    public RenderFragment<BootstrapRenderSubmitInfo>? RenderSubmit { get; set; }
    #endregion
    #region 是否仅显示
    /// <inheritdoc cref="FormViewer{Model}.IsReadOnly"/>
    [Parameter]
    public Func<PropertyInfo, Model, bool> IsReadOnly { get; set; } = (_, _) => false;
    #endregion
    #region 是否显示提交部分
    /// <inheritdoc cref="FormViewer{Model}.ShowSubmit"/>
    [Parameter]
    public Func<Model, bool> ShowSubmit { get; set; } = _ => true;
    #endregion
    #region 刷新目标
    /// <summary>
    /// 获取刷新目标，
    /// 它决定了在提交，重置，删除，取消表单时，应该刷新哪个组件
    /// </summary>
    [Parameter]
    public IHandleEvent RefreshTarget { get; set; }
    #endregion
    #endregion
    #region 关于模型
    #region 用来初始化模型的委托
    /// <inheritdoc cref="FormViewer{Model}.InitializationModel"/>
    [Parameter]
    public Func<Task<Model>> InitializationModel { get; set; }
        = () => Task.FromResult<Model>(new());
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
    public Func<IEnumerable<PropertyInfo>, FormViewer<Model>, IEnumerable<RenderFormViewerPropertyInfo<Model>>> ToPropertyInfo { get; set; } = FormViewer<Model>.ToPropertyInfoDefault;
    #endregion
    #region 数据属性改变时的委托
    /// <inheritdoc cref="FormViewer{Model}.OnPropertyChangeed"/>
    [Parameter]
    public Func<RenderFormViewerPropertyInfo<Model>, Task> OnPropertyChangeed { get; set; } = _ => Task.CompletedTask;
    #endregion
    #endregion
    #region 关于业务逻辑
    #region 验证业务逻辑
    #region 用来验证数据的委托
    /// <inheritdoc cref="FormViewer{Model}.Verify"/>
    [Parameter]
    public DataVerify Verify { get; set; } = FormViewer<Model>.VerifyDefault;
    #endregion
    #region 验证失败时的委托
    /// <summary>
    /// 当验证失败时，执行的委托，
    /// 它的参数就是验证结果
    /// </summary>
    [Parameter]
    public Func<VerificationResults, Task> VerifyFail { get; set; } = _ => Task.CompletedTask;
    #endregion
    #endregion
    #region 用来提交表单的业务逻辑
    /// <summary>
    /// 获取或设置用于提交表单的业务逻辑，
    /// 它的参数就是当前表单的模型以及验证结果，
    /// 返回值是业务逻辑是否成功
    /// </summary>
    [Parameter]
    public Func<Model, Task<bool>> Submit { get; set; } = _ => Task.FromResult(true);
    #endregion
    #region 是否提交后自动重置表单
    /// <summary>
    /// 获取提交后是否自动重置表单
    /// </summary>
    [Parameter]
    public bool ResetAfterSubmission { get; set; }
    #endregion
    #region 用来重置表单的业务逻辑
    /// <summary>
    /// 获取或设置用于重置表单的业务逻辑，
    /// 当执行这个委托的时候，模型已被恢复为初始状态
    /// </summary>
    [Parameter]
    public Func<Task> Resetting { get; set; } = () => Task.CompletedTask;
    #endregion
    #region 用来删除表单的业务逻辑
    /// <summary>
    /// 获取或设置用于删除表单的业务逻辑，
    /// 它的参数就是当前表单的模型以及验证结果，
    /// 返回值是业务逻辑是否成功
    /// </summary>
    [Parameter]
    public Func<Model, Task<bool>> Delete { get; set; } = _ => Task.FromResult(true);
    #endregion
    #region 用来取消表单的业务逻辑
    /// <summary>
    /// 用于取消表单，回到上级页面的业务逻辑，
    /// 如果不指定它，则不渲染取消按钮
    /// </summary>
    [Parameter]
    public Func<Task>? Cancellation { get; set; }
    #endregion
    #endregion 
    #endregion
    #region 内部成员
    #region 获取提交部分的渲染参数
    /// <summary>
    /// 获取用来渲染提交部分的渲染参数
    /// </summary>
    /// <param name="info">基础版本的渲染参数</param>
    /// <returns></returns>
    private BootstrapRenderSubmitInfo GetRenderSubmitInfo(RenderSubmitInfo info)
        => new()
        {
            Delete = info.ExistingForms ?
                new(RefreshTarget, async () =>
                {
                    var data = info.ModelAndVerify();
                    var isSuccess = await Delete((Model)data.Data);
                    if ((isSuccess, Cancellation) is (true, { } cancellation))
                        await cancellation();
                }) : null,
            Resetting = new(RefreshTarget, async () =>
            {
                await info.Resetting();
                await Resetting();
            }),
            Submit = new(RefreshTarget, async () =>
            {
                var data = info.ModelAndVerify();
                if (!data.IsSuccess)
                {
                    await VerifyFail(data);
                    return;
                }
                var isSuccess = await Submit((Model)data.Data);
                if (isSuccess)
                {
                    if (Cancellation is { } cancellation)
                        await cancellation();
                    else
                    {
                        if (ResetAfterSubmission)
                            await info.Resetting();
                    }
                }
            }),
            Cancellation = Cancellation is null ?
                null :
                new(RefreshTarget, Cancellation)
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
