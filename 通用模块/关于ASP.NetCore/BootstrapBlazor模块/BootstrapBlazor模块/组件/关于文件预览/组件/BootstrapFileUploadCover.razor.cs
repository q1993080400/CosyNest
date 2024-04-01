using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本组件是为上传文件优化过的封面，
/// 可以被放置到<see cref="BootstrapFileViewer.RenderCover"/>中，
/// 点击它不会预览，而是会删除这个文件
/// </summary>
public sealed partial class BootstrapFileUploadCover : ComponentBase
{
    #region 组件参数
    #region 用来渲染封面的参数
    /// <summary>
    /// 获取用来渲染封面的参数
    /// </summary>
    [Parameter]
    [EditorRequired]
    public BootstrapRenderCoverInfo RenderCoverInfo { get; set; }
    #endregion
    #region 点击封面时触发的事件
    /// <summary>
    /// 当点击封面时，触发这个事件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public EventCallback<FileSource> ClickEvent { get; set; }
    #endregion
    #endregion
}
