namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件是一个文本呈现，它能够正确地识别文本的换行符
/// </summary>
public sealed partial class TextRendering : ComponentBase
{
    #region 文本值
    /// <summary>
    /// 获取或设置要呈现的值，
    /// 它会调用<see cref="object.ToString"/>方法来获取要呈现的文本
    /// </summary>
    [EditorRequired]
    [Parameter]
    public object? Value { get; set; }
    #endregion
}
