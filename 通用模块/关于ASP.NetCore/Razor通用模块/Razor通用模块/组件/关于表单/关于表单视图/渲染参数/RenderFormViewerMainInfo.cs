using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="FormViewer{Model}"/>主体部分的参数，
/// 主体部分指的是表单部分，不包括提交重置按钮等
/// </summary>
public sealed record RenderFormViewerMainInfo<Model>
    where Model : class
{
    #region 获取当前模型
    /// <summary>
    /// 获取当前要渲染的模型
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
    #region 渲染委托
    /// <summary>
    /// 获取渲染这个部分的委托，
    /// 它可以用来在常规渲染之外增加别的UI
    /// </summary>
    public required RenderFragment Render { get; init; }
    #endregion
}
