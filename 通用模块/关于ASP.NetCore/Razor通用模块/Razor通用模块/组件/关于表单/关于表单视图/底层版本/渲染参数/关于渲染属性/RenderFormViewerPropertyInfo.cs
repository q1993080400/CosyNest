using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="FormViewer{Model}"/>的中每个属性的参数
/// </summary>
/// <inheritdoc cref="RenderFormViewerPropertyInfoBase{Model}"/>
public sealed record RenderFormViewerPropertyInfo<Model>: RenderFormViewerPropertyInfoBase<Model>
    where Model : class
{
    #region 属性名称
    /// <summary>
    /// 获取要渲染的属性的名称，
    /// 注意：它不一定是<see cref="PropertyInfo"/>的名称，
    /// 它会被显示在UI上
    /// </summary>
    public required string PropertyName { get; init; }
    #endregion
}
