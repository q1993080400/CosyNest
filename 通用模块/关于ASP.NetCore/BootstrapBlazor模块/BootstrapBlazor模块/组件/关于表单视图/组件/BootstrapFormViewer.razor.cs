using System.Reflection;

using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本组件是<see cref="FormViewer{Model}"/>的开箱即用版，
/// 它底层由Bootstrap实现
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed partial class BootstrapFormViewer<Model> : ComponentBase
    where Model : class
{
    #region 组件参数
    #region 关于渲染
    #region 用来渲染每个属性的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderProperty"/>
    [Parameter]
    public RenderFragment<RenderFormViewerPropertyInfoBase<Model>>? RenderProperty { get; set; }
    #endregion
    #region 用来渲染整个组件的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderComponent"/>
    [Parameter]
    public RenderFragment<RenderFormViewerInfo<Model>>? RenderComponent { get; set; }
    #endregion
    #region 用来渲染主体部分的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderMain"/>
    [Parameter]
    public RenderFragment<RenderFormViewerMainInfo<Model>>? RenderMain { get; set; }
    #endregion
    #region 用来渲染提交部分的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderSubmit"/>
    [Parameter]
    public RenderFragment<RenderBusinessFormViewerInfo<Model>>? RenderSubmit { get; set; }
    #endregion
    #region 是否可编辑
    /// <inheritdoc cref="FormViewer{Model}.CanEdit"/>
    [Parameter]
    public bool CanEdit { get; set; }
    #endregion
    #region 是否仅显示
    /// <inheritdoc cref="FormViewer{Model}.IsReadOnlyProperty"/>
    [Parameter]
    public Func<PropertyInfo, Model, bool> IsReadOnlyProperty { get; set; }
        = FormViewer<Model>.PropertyStateJudge;
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
    public Func<Model, bool> ExistingForms { get; set; }
        = FormViewer<Model>.ExistingFormsDefault;
    #endregion
    #region 用来获取属性渲染参数的委托
    /// <inheritdoc cref="FormViewer{Model}.GetRenderPropertyInfo"/>
    [Parameter]
    public Func<Type, FormViewer<Model>, IEnumerable<RenderFormViewerPropertyInfoBase<Model>>> GetRenderPropertyInfo { get; set; }
        = FormViewer<Model>.GetRenderPropertyInfoDefault;
    #endregion
    #region 数据属性改变时的委托
    /// <inheritdoc cref="FormViewer{Model}.OnPropertyChangeed"/>
    [Parameter]
    public Func<RenderFormViewerPropertyInfoBase<Model>, Task> OnPropertyChangeed { get; set; } = _ => Task.CompletedTask;
    #endregion
    #endregion
    #region 关于业务逻辑
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
}
