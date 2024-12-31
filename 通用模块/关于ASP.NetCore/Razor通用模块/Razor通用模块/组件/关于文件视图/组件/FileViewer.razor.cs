using System.Media;

using Microsoft.AspNetCore.Components.Rendering;

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
    #region 内部成员
    #region 预览渲染参数
    /// <summary>
    /// 获取用来渲染预览的参数，
    /// 如果为<see langword="null"/>，
    /// 表示目前没有预览
    /// </summary>
    private RenderFilePreviewInfo? RenderFilePreviewInfo { get; set; }
    #endregion
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderFileViewerInfo GetRenderInfo()
    {
        var renderFiles = Files.WhereEnable().ToArray();
        var renderFileViewerInfos = renderFiles.Select((file, index) =>
        {
            #region 判断是否可预览的本地函数
            static bool CanPreview(IHasReadOnlyPreviewFile file)
             => file is { IsEnable: true, MediumFileType: MediumFileType.Image or MediumFileType.Video or MediumFileType.Audio };
            #endregion
            #region 用于打开预览的本地函数
            void OpenPreview()
            {
                #region 枚举预览文件的本地函数
                IEnumerable<IHasReadOnlyPreviewFile> PreviewFile()
                => renderFiles[index..].Concat(renderFiles[..index]).
                Where(CanPreview);
                #endregion
                #region 用于关闭预览的本地函数
                void ClosePreview()
                => RenderFilePreviewInfo = null;
                #endregion
                RenderFilePreviewInfo = new()
                {
                    PreviewFile = PreviewFile(),
                    ClosePreview = new(this, ClosePreview)
                };
            }
            #endregion
            var canPreview = CanPreview(file);
            #region 取消选择文件的本地函数
            void CancelFile()
            => file.Disable();
            #endregion
            var renderSingleFileInfo = new RenderSingleFileInfo()
            {
                File = file,
                Index = index,
                CanPreview = canPreview,
                OpenPreview = canPreview ?
                new(this, OpenPreview) :
                CreateRazor.EventCallbackNotRefresh(),
                CancelFile = new(this, CancelFile)
            };
            return RenderFile(renderSingleFileInfo);
        }).ToArray();
        #region 用来渲染预览文件的本地函数
        void RenderFilePreviewFunction(RenderTreeBuilder builder)
        {
            if ((RenderFilePreview, RenderFilePreviewInfo) is ({ }, { }))
                RenderFilePreview(RenderFilePreviewInfo)(builder);
        }
        #endregion
        return new()
        {
            RenderFiles = renderFileViewerInfos,
            RenderFilePreview = RenderFilePreviewFunction
        };
    }
    #endregion
    #endregion
}
