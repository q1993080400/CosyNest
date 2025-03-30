namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个枚举指示在渲染<see cref="FileViewer"/>组件的文件时，
/// 应该为它附加什么onclick事件
/// </summary>
public enum RenderSingleFileEventPreference
{
    /// <summary>
    /// 指示不改变onclick事件
    /// </summary>
    None,
    /// <summary>
    /// 指示将onclick事件改为取消对文件的选择
    /// </summary>
    Cancel,
    /// <summary>
    /// 指示将onclick事件改为预览文件
    /// </summary>
    Preview
}
