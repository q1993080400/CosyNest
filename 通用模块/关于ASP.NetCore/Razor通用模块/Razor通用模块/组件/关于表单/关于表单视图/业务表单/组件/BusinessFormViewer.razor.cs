using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是表单视图的特化版本，
/// 它耦合更强，但是显著增强了渲染提交部分的能力
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed partial class BusinessFormViewer<Model> : ComponentBase
    where Model : class
{
    #region 组件参数
    #region 关于渲染
    #region 用来渲染每个属性的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderProperty"/>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFormViewerPropertyInfoBase<Model>> RenderProperty { get; set; }
    #endregion
    #region 用来渲染整个组件的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderComponent"/>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFormViewerInfo<Model>> RenderComponent { get; set; }
    #endregion
    #region 用来渲染主体部分的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderMain"/>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFormViewerMainInfo<Model>> RenderMain { get; set; }
    #endregion
    #region 用来渲染提交部分的委托
    /// <inheritdoc cref="FormViewer{Model}.RenderSubmit"/>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderBusinessFormViewerInfo<Model>> RenderSubmit { get; set; }
    #endregion
    #region 表单是否可编辑
    /// <inheritdoc cref="FormViewer{Model}.CanEditForm"/>
    [Parameter]
    public bool? CanEditForm { get; set; }
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
    #region 值转换函数
    /// <inheritdoc cref="FormViewer{Model}.PropertyValueConvert"/>
    [Parameter]
    public Func<Type, object?, object?>? PropertyValueConvert { get; set; }
    #endregion
    #region 创建值改变时的函数的高阶函数
    /// <inheritdoc cref="FormViewer{Model}.CreatePropertyChangeEvent"/>
    [Parameter]
    public Func<RenderFormViewerPropertyInfoBase<Model>, Func<object?, Task>?>? CreatePropertyChangeEvent { get; set; }
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
    /// <summary>
    /// 获取或设置用于提交表单的业务逻辑，
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
    private RenderBusinessFormViewerInfo<Model> GetRenderSubmitInfo(RenderSubmitInfo<Model> info)
        => new()
        {
            BaseRenderInfo = info with
            {
                Resetting = async () =>
                {
                    await info.Resetting();
                    await Resetting();
                }
            },
            Delete = info.ExistingForms ?
                 async () =>
                 {
                     var isSuccess = await Delete(info.FormModel);
                     if ((isSuccess, Cancellation) is (true, { } cancellation))
                         await cancellation();
                 }
            : null,
            Submit = async () =>
            {
                var model = info.FormModel;
                var isSuccess = await Submit(model);
                if (isSuccess)
                {
                    if (Cancellation is { } cancellation)
                        await cancellation();
                    else if (ResetAfterSubmission)
                    {
                        await info.Resetting();
                    }
                }
            },
            Cancellation = Cancellation is null ?
                null :
                Cancellation
        };
    #endregion
    #endregion
}
