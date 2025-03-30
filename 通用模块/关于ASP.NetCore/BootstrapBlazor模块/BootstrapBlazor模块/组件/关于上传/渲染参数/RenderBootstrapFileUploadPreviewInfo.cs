using System.DataFrancis;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个记录是渲染<see cref="BootstrapFileUpload"/>组件预览部分的参数
/// </summary>
public sealed record RenderBootstrapFileUploadPreviewInfo
{
    #region 所有文件集合
    /// <summary>
    /// 获取所有以前和现在上传的文件的集合，
    /// 它已经对<see cref="IHasReadOnlyPreviewFile.IsEnable"/>进行过筛选
    /// </summary>
    public required IReadOnlyList<IHasReadOnlyPreviewFile> Files { get; init; }
    #endregion
}
