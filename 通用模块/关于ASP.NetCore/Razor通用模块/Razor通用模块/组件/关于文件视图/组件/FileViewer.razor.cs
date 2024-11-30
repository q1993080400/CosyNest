using System.Media;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个文件视图，
/// 它被用来显示和预览文件
/// </summary>
public sealed partial class FileViewer : ComponentBase
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
    /// 用来渲染整个组件的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFileViewerInfo> RenderComponent { get; set; }
    #endregion
    #region 用来渲染单个文件的委托
    /// <summary>
    /// 用来渲染单个文件的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderSingleFileInfo> RenderFile { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderFileViewerInfo GetRenderInfo()
    {
        var renderFiles = Files.Where(x => x is not ICanCancelPreviewFile { IsEnable: false }).ToArray();
        var renderFileViewerInfos = renderFiles.Select((file, index) =>
        {
            #region 枚举预览文件的本地函数
            IEnumerable<IHasReadOnlyPreviewFile> PreviewFile()
            => renderFiles[index..].Concat(renderFiles[..index]).
            Where(x => x.MediumFileType is MediumFileType.Image or MediumFileType.Video or MediumFileType.Audio);
            #endregion
            var renderSingleFileInfo = new RenderSingleFileInfo()
            {
                File = file,
                Index = index,
                PreviewFile = PreviewFile()
            };
            return RenderFile(renderSingleFileInfo);
        }).ToArray();
        return new()
        {
            RenderAllFile = renderFileViewerInfos
        };
    }
    #endregion
    #endregion
}
