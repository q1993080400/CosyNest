﻿namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="FormViewer{Model}"/>的中每个属性的参数
/// </summary>
/// <inheritdoc cref="RenderFormViewerPropertyInfoBase{Model}"/>
public sealed record RenderFormViewerPropertyInfo<Model> : RenderFormViewerPropertyInfoBase<Model>, ITitleData
    where Model : class
{
    #region 渲染偏好
    /// <summary>
    /// 获取进行渲染时的偏好
    /// </summary>
    public required RenderPreference RenderPreference { get; init; }
    #endregion
    #region 属性名称
    public required string Name { get; init; }
    #endregion
    #region 获取属性的值的类型
    public Type ValueType
         => Property.PropertyType;
    #endregion
}
