namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="FormViewer{Model}"/>整个组件的参数
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed record RenderFormViewerInfo<Model>
    where Model : class
{
    #region 用来渲染主体部分的委托
    /// <summary>
    /// 获取用来渲染主体部分的委托，
    /// 主体部分指的是表单的所有属性部分
    /// </summary>
    public required RenderFragment RenderMain { get; init; }
    #endregion
    #region 用来渲染提交部分的委托
    /// <summary>
    /// 获取用来渲染提交部分的委托，
    /// 提交部分指的是包含提交，删除，重置等按钮的区域
    /// </summary>
    public required RenderFragment RenderSubmit { get; init; }
    #endregion
    #region 获取当前模型
    /// <summary>
    /// 获取当前要渲染的模型
    /// </summary>
    public required Model FormModel { get; init; }
    #endregion
}
