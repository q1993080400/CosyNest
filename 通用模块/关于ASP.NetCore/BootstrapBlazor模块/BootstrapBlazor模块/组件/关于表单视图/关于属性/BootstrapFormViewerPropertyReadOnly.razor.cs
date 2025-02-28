using System.DataFrancis;

using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本组件是用来渲染<see cref="BootstrapFormViewer{Model}"/>只读属性的默认方法
/// </summary>
public sealed partial class BootstrapFormViewerPropertyReadOnly : ComponentBase
{
    #region 组件参数
    #region 标题
    /// <summary>
    /// 获取这个属性的标题，
    /// 如果为<see langword="null"/>，
    /// 表示不渲染标题
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string? Title { get; set; }
    #endregion
    #region 属性的值
    /// <summary>
    /// 获取属性的值
    /// </summary>
    [Parameter]
    [EditorRequired]
    public object? Value { get; set; }
    #endregion
    #region 渲染偏好
    /// <summary>
    /// 获取属性的渲染偏好
    /// </summary>
    [Parameter]
    public RenderPreference? RenderPreference { get; set; }
    #endregion
    #region 值为null的时候显示的文本
    /// <summary>
    /// 当<see cref="Value"/>为<see langword="null"/>的时候，
    /// 显示这个文本
    /// </summary>
    [Parameter]
    public string? IfNullDescription { get; set; }
    #endregion
    #region 用来进行递归渲染的参数
    /// <summary>
    /// 获取用来进行递归渲染的参数，
    /// 如果为<see langword="null"/>，
    /// 表示它不是递归渲染
    /// </summary>
    [Parameter]
    public RenderFormViewerPropertyInfoRecursion? RecursionInfo { get; set; }
    #endregion
    #region 级联参数：用来进行递归渲染的委托
    /// <summary>
    /// 获取用来递归渲染属性的委托
    /// </summary>
    [CascadingParameter]
    private RenderFragment<RenderFormViewerPropertyInfoRecursion>? RenderRecursion { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 将值格式化为文本
    /// <summary>
    /// 将<see cref="Value"/>格式化为文本，
    /// 它会被作为最终显示的文本
    /// </summary>
    /// <returns></returns>
    private string? FormatValue()
        => (Value, RenderPreference) switch
        {
            (null, _) => IfNullDescription,
            (var value, { } renderPreference) => renderPreference.RenderToText(value),
            (Enum @enum, _) => @enum.GetDescription(),
            (var value, _) => value.ToString()
        };
    #endregion
    #region 复制文字
    /// <summary>
    /// 如果值是一个文本，则复制它
    /// </summary>
    /// <returns></returns>
    private async Task CopyText()
    {
        var text = Value as string;
        if (text.IsVoid())
            return;
        var isSuccess = await JSWindow.Navigator.Clipboard.WriteText(text);
        await MessageService.Show(isSuccess ? "复制成功" : "复制失败");
    }
    #endregion
    #endregion
}
