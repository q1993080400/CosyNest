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
    /// 这个委托的参数是要向文本框写入的新文本，
    /// 返回值是一个脚本，它可以写入该文本，
    /// 并自动调整文本框的大小
    /// </summary>
    public required Func<string?, string> ChangeTextScript { get; init; }
    #endregion
}
