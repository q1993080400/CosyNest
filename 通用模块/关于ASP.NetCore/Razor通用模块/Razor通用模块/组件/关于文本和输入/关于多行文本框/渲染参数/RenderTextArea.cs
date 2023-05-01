namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是<see cref="TextAreaFrancis"/>组件的渲染参数
/// </summary>
public sealed record RenderTextArea
{
    #region 组件的ID
    /// <inheritdoc cref="TextAreaFrancis.ID"/>
    public required string ID { get; init; }
    #endregion
    #region OnInput事件触发时执行的脚本
    /// <inheritdoc cref="TextAreaFrancis.OnInput"/>
    public required string OnInput { get; init; }
    #endregion
    #region 使用编程方式改变文本
    /// <summary>
    /// 当使用编程方式改变多行文本框的文本时，
    /// 如果需要自动改变文本框的大小，请调用这个委托，
    /// 它的第一个参数是JS运行时对象，第二个参数是要写入的新文本
    /// </summary>
    public required Func<IJSWindow, string?, Task> ChangeText { get; init; }
    #endregion
}
