﻿using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="FormViewer{Model}"/>的中每个属性的参数
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed record RenderFormViewerPropertyInfo<Model>
    where Model : class
{
    #region 要渲染的模型
    /// <summary>
    /// 获取要渲染的模型
    /// </summary>
    public required Model FormModel { get; init; }
    #endregion
    #region 要渲染的属性
    /// <summary>
    /// 获取要渲染的属性
    /// </summary>
    public required PropertyInfo Property { get; init; }
    #endregion
    #region 属性名称
    /// <summary>
    /// 获取要渲染的属性的名称，
    /// 注意：它不一定是<see cref="PropertyInfo"/>的名称，
    /// 它会被显示在UI上
    /// </summary>
    public required string PropertyName { get; init; }
    #endregion
    #region 创建属性绑定对象
    /// <summary>
    /// 创建一个属性绑定对象，
    /// 它允许绑定表单属性
    /// </summary>
    /// <typeparam name="Value">属性的类型</typeparam>
    /// <returns></returns>
    public BindPropertyInfo<Value> Value<Value>()
        => new(FormModel, Property, () => OnPropertyChangeed(this));
    #endregion
    #region 是否仅显示
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示仅提供数据显示功能，不提供数据编辑功能
    /// </summary>
    public required bool IsReadOnly { get; init; }
    #endregion
    #region 数据属性改变时的委托
    /// <summary>
    /// 当数据属性改变时，执行这个委托，
    /// 它的参数就是当前属性渲染参数
    /// </summary>
    public required Func<RenderFormViewerPropertyInfo<Model>, Task> OnPropertyChangeed { get; init; }
    #endregion
}
