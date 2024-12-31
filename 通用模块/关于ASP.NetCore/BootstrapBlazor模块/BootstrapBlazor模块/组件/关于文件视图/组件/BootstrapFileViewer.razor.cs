using System.DataFrancis;

using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本组件是底层由Bootstrap实现的文件视图组件，
/// 它可以用来显示和预览文件
/// </summary>
public sealed partial class BootstrapFileViewer : ComponentBase
{
    #region 组件参数
    #region 要预览的文件
    /// <summary>
    /// 获取要预览的所有文件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IEnumerable<IHasReadOnlyPreviewFile> Files { get; set; }
    #endregion
    #region 用来渲染整个组件的委托
    /// <summary>
    /// 用来渲染整个组件的委托，
    /// 如果为<see langword="null"/>，则使用默认方法
    /// </summary>
    [Parameter]
    public RenderFragment<RenderFileViewerInfo>? RenderComponent { get; set; }
    #endregion
    #region 用来渲染单个文件的委托
    /// <summary>
    /// 用来渲染单个文件的委托，
    /// 如果为<see langword="null"/>，则使用默认方法
    /// </summary>
    [Parameter]
    public RenderFragment<RenderSingleFileInfo>? RenderFile { get; set; }
    #endregion
    #region 用来渲染预览文件的委托
    /// <summary>
    /// 用来渲染预览文件的委托，
    /// 如果为<see langword="null"/>，
    /// 则不渲染预览
    /// </summary>
    [Parameter]
    public RenderFragment<RenderFilePreviewInfo>? RenderFilePreview { get; set; }
    #endregion
    #endregion
}
