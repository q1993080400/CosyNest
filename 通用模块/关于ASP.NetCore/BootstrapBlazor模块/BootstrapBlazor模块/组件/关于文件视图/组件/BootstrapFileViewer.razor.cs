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
    #region 是否填满剩余空间
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则最后一个子元素会填满这一行的所有剩余空间，
    /// 在这种情况下，不能使用gap属性，只能使用padding属性，否则会出现问题，
    /// 默认为<see langword="true"/>
    /// </summary>
    [Parameter]
    public bool FillRemainingSpace { get; set; } = true;
    #endregion
    #endregion
}
