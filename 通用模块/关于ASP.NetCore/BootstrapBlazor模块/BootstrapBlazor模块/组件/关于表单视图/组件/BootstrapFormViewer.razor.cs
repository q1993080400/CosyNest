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
    #region 用来进行递归渲染的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderRecursion"/>
    [Parameter]
    public RenderFragment<RenderFormViewerPropertyInfoRecursion>? RenderRecursion { get; set; }
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
    public RenderFragment<RenderSubmitInfo<Model>>? RenderSubmit { get; set; }
    #endregion
    #region 是否强制所有属性只读
    /// <inheritdoc cref="FormViewer{Model}.ForceReadOnly"/>
    [Parameter]
    public bool ForceReadOnly { get; set; }
    #endregion
    #region 是否只读
    /// <inheritdoc cref="FormViewer{Model}.IsReadOnlyProperty"/>
    [Parameter]
    public Func<PropertyInfo, Model, bool> IsReadOnlyProperty { get; set; }
        = FormViewer<Model>.IsReadOnlyPropertyDefault;
    #endregion
    #region 用来渲染区域的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderRegion"/>
    [Parameter]
    public RenderFragment<RenderFormViewerRegionInfo<Model>> RenderRegion { get; set; }
    #endregion
    #region 用来渲染上传遮罩的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderUploadMask"/>
    [Parameter]
    public RenderFragment? RenderUploadMask { get; set; }
    #endregion
    #region 是否自动渲染遮罩区域
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则自动渲染遮罩区域，否则你需要手动渲染它
    /// </summary>
    [Parameter]
    public bool AutoRenderUploadMask { get; set; } = true;
    #endregion
    #endregion
    #region 关于模型
    #region 初始模型
    /// <inheritdoc cref="FormViewer{Model}.InitializationModel"/>
    [Parameter]
    [EditorRequired]
    public Model InitializationModel { get; set; }
    #endregion
    #region 是否引用传递
    /// <inheritdoc cref="FormViewer{Model}.IsReference"/>
    [Parameter]
    public bool IsReference { get; set; }
    #endregion
    #region 用来判断是否为现有表单的委托
    /// <inheritdoc cref="FormViewer{Model}.ExistingForm"/>
    [Parameter]
    public Func<Model, bool> ExistingForm { get; set; }
        = FormViewer<Model>.ExistingFormDefault;
    #endregion
    #region 创建值改变时的函数的高阶函数
    /// <inheritdoc cref="FormViewer{Model}.CreatePropertyChangeEvent"/>
    [Parameter]
    public Func<RenderFormViewerPropertyInfoBase<Model>, Func<object?, Task>?>? CreatePropertyChangeEvent { get; set; }
    #endregion
    #region 用来筛选要渲染的属性的委托
    /// <inheritdoc cref="FormViewer{Model}.FilterRenderPropertyInfo"/>
    [Parameter]
    public Func<Type, IEnumerable<PropertyInfo>> FilterRenderPropertyInfo { get; set; } = FormViewer<Model>.FilterRenderPropertyInfoDefault;
    #endregion
    #region 用来获取属性渲染参数的委托
    /// <inheritdoc cref="FormViewer{Model}.GetRenderPropertyInfo"/>
    [Parameter]
    public Func<FormViewer<Model>, IEnumerable<RenderFormViewerPropertyInfoBase<Model>>> GetRenderPropertyInfo { get; set; }
        = FormViewer<Model>.GetRenderPropertyInfoDefault;
    #endregion
    #endregion
    #region 关于业务逻辑
    #region 用来提交表单的业务逻辑
    /// <inheritdoc cref="FormViewer{Model}.Submit"/>
    [Parameter]
    public Func<Model, Task<bool>> Submit { get; set; } = static _ => Task.FromResult(true);
    #endregion
    #region 是否提交后自动重置表单
    /// <inheritdoc cref="FormViewer{Model}.ResetAfterSubmission"/>
    [Parameter]
    public bool ResetAfterSubmission { get; set; }
    #endregion
    #region 用来重置表单的业务逻辑
    /// <inheritdoc cref="FormViewer{Model}.Resetting"/>
    [Parameter]
    public Func<Task> Resetting { get; set; } = static () => Task.CompletedTask;
    #endregion
    #region 用来删除表单的业务逻辑
    /// <inheritdoc cref="FormViewer{Model}.Delete"/>
    [Parameter]
    public Func<Model, Task<bool>>? Delete { get; set; }
    #endregion
    #region 用来取消表单的业务逻辑
    /// <inheritdoc cref="FormViewer{Model}.Cancellation"/>
    [Parameter]
    public Func<Task>? Cancellation { get; set; }
    #endregion
    #region 刷新目标
    /// <inheritdoc cref="FormViewer{Model}.RefreshTarget"/>
    [Parameter]
    public ComponentBase? RefreshTarget { get; set; }
    #endregion
    #endregion
    #endregion
    #region 构造函数
    public BootstrapFormViewer()
    {
        Resetting = () =>
        {
            this.StateHasChanged();
            return Task.CompletedTask;
        };
    }
    #endregion
}
