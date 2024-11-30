namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="FileViewer"/>组件中单个文件的参数
/// </summary>
public sealed record RenderSingleFileInfo
{
    #region 要渲染的文件
    /// <summary>
    /// 获取要渲染的文件
    /// </summary>
    public required IHasReadOnlyPreviewFile File { get; init; }
    #endregion
    #region 文件的索引
    /// <summary>
    /// 获取这个文件在所有文件中的索引
    /// </summary>
    public required int Index { get; init; }
    #endregion
    #region 预览文件的集合
    /// <summary>
    /// 假设从本文件开始预览文件，
    /// 则依次返回应该预览的文件列表
    /// </summary>
    public required IEnumerable<IHasReadOnlyPreviewFile> PreviewFile { get; init; }
    #endregion
}
