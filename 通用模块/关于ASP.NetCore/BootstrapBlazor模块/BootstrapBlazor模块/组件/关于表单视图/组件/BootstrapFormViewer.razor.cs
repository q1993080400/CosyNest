using System.DataFrancis.EntityDescribe;
using System.Reflection;

using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本组件是<see cref="FormViewer{Model}"/>的开箱即用版，
/// 它底层由Bootstrap实现
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed partial class BootstrapFormViewer<Model> : ComponentBase, IContentComponent<RenderFragment<RenderFormViewerInfo<Model>>?>
    where Model : class
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
    public RenderFragment<RenderBusinessFormViewerInfo<Model>>? RenderSubmit { get; set; }
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
    /// <inheritdoc cref="BusinessFormViewer{Model}.RefreshTarget"/>
    [Parameter]
    public IHandleEvent RefreshTarget { get; set; }
    #endregion
    #endregion
    #region 关于模型
    #region 用来初始化模型的委托
    /// <inheritdoc cref="FormViewer{Model}.InitializationModel"/>
    [Parameter]
    [EditorRequired]
    public Func<Task<Model>> InitializationModel { get; set; }
    #endregion
    #region 模型是否按值引用
    /// <inheritdoc cref="FormViewer{Model}.IsValueReference"/>
    [Parameter]
    public bool IsValueReference { get; set; } = true;
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
    /// <inheritdoc cref="BusinessFormViewer{Model}.VerifyFail"/>
    [Parameter]
    public Func<VerificationResults, Task> VerifyFail { get; set; } = _ => Task.CompletedTask;
    #endregion
    #endregion
    #region 用来提交表单的业务逻辑
    /// <inheritdoc cref="BusinessFormViewer{Model}.Submit"/>
    [Parameter]
    public Func<Model, Task<bool>> Submit { get; set; } = _ => Task.FromResult(true);
    #endregion
    #region 是否提交后自动重置表单
    /// <inheritdoc cref="BusinessFormViewer{Model}.ResetAfterSubmission"/>
    [Parameter]
    public bool ResetAfterSubmission { get; set; }
    #endregion
    #region 用来重置表单的业务逻辑
    /// <inheritdoc cref="BusinessFormViewer{Model}.Resetting"/>
    [Parameter]
    public Func<Task> Resetting { get; set; } = () => Task.CompletedTask;
    #endregion
    #region 用来删除表单的业务逻辑
    /// <inheritdoc cref="BusinessFormViewer{Model}.Delete"/>
    [Parameter]
    public Func<Model, Task<bool>> Delete { get; set; } = _ => Task.FromResult(true);
    #endregion
    #region 用来取消表单的业务逻辑
    /// <inheritdoc cref="BusinessFormViewer{Model}.Cancellation"/>
    [Parameter]
    public Func<Task>? Cancellation { get; set; }
    #endregion
    #endregion 
    #endregion
    #region 构造函数
    public BootstrapFormViewer()
    {
        RefreshTarget = this;
    }
    #endregion
}
