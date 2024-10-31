namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录表示应该使用完全自定义的方式，
/// 渲染<see cref="FormViewer{Model}"/>的每个属性
/// </summary>
/// <inheritdoc cref="RenderFormViewerPropertyInfoBase{Model}"/>
public sealed record RenderFormViewerPropertyInfoCustom<Model> : RenderFormViewerPropertyInfoBase<Model>
    where Model : class
{
}
