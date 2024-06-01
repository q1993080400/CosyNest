namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是<see cref="TextReader"/>的开箱即用版，
/// 注意：它不负责渲染文本的容器
/// </summary>
public sealed partial class TextRenderingSimple : ComponentBase
{
    #region 组件参数
    #region 文本值
    /// <summary>
    /// 获取或设置要呈现的值，
    /// 它会调用<see cref="object.ToString"/>方法来获取要呈现的文本
    /// </summary>
    [EditorRequired]
    [Parameter]
    public object? Value { get; set; }
    #endregion
    #region 高亮文本
    /// <summary>
    /// 获取高亮文本的集合
    /// </summary>
    [Parameter]
    public IReadOnlyCollection<string>? Highlight { get; set; }
    #endregion
    #endregion
}
