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
    /// 获取属性的值，
    /// 如果属性不是文件或文件的集合，
    /// 可以不指定它
    /// </summary>
    [Parameter]
    public object? Value { get; set; }
    #endregion
    #region 格式化后的文本
    /// <summary>
    /// 获取格式化后的文本，
    /// 它会被作为显示在UI上的文本
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string? FormatValue { get; set; }
    #endregion
    #region 说明文本
    /// <summary>
    /// 获取对这个属性的说明
    /// </summary>
    [Parameter]
    public string? Describe { get; set; }
    #endregion
    #region 渲染偏好
    /// <summary>
    /// 获取属性的渲染偏好
    /// </summary>
    [Parameter]
    public RenderPreference? RenderPreference { get; set; }
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
    #region 复制文字
    /// <summary>
    /// 如果值是一个文本，则复制它
    /// </summary>
    /// <returns></returns>
    private async Task CopyText()
    {
        if (FormatValue.IsVoid())
            return;
        var isSuccess = await JSWindow.Navigator.Clipboard.WriteText(FormatValue);
        await MessageService.Show(isSuccess ? "复制成功" : "复制失败");
    }
    #endregion
    #endregion
}
