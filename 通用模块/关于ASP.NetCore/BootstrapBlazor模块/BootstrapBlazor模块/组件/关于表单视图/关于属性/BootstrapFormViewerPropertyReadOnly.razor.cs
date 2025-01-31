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
    /// 获取这个属性的标题
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Title { get; set; }
    #endregion
    #region 属性的值
    /// <summary>
    /// 获取属性的值
    /// </summary>
    [Parameter]
    [EditorRequired]
    public object? Value { get; set; }
    #endregion
    #region 格式
    /// <summary>
    /// 获取<see cref="Value"/>的格式化字符串，
    /// 仅对部分类型有效
    /// </summary>
    [Parameter]
    public string? Format { get; set; }
    #endregion
    #region 值为null的时候显示的文本
    /// <summary>
    /// 当<see cref="Value"/>为<see langword="null"/>的时候，
    /// 显示这个文本
    /// </summary>
    [Parameter]
    public string? IfNullDescription { get; set; }
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
        => (Value, Format) switch
        {
            (Enum @enum, _) => @enum.GetDescription(),
            (IFormattable formattable, var formatText) => (formattable, formatText) switch
            {
                (decimal num, null) => num.ToString(System.Format.FormattedNumCommon),
                (DateTimeOffset date, null) => date.ToString("d"),
                _ => formattable.ToString(formatText, null)
            },
            var (v, _) => v?.ToString() ?? IfNullDescription
        };
    #endregion
    #endregion
}
