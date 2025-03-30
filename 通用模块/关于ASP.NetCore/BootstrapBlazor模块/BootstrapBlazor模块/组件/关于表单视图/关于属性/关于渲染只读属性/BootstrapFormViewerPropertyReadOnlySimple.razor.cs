using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本组件是用来渲染<see cref="BootstrapFormViewer{Model}"/>只读属性的简单方法
/// </summary>
/// <inheritdoc cref="BootstrapFormViewer{Model}"/>
public sealed partial class BootstrapFormViewerPropertyReadOnlySimple<Model> : ComponentBase
    where Model : class
{
    #region 组件参数
    #region 渲染参数
    /// <summary>
    /// 获取渲染这个只读属性的参数
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFormViewerPropertyInfo<Model> RenderPropertyInfo { get; set; }
    #endregion
    #endregion
}
