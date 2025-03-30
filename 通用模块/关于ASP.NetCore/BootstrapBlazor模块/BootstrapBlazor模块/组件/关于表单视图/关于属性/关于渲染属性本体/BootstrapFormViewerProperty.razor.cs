using System.DataFrancis;

using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本组件是用来渲染<see cref="BootstrapFormViewer{Model}"/>属性的默认方法
/// </summary>
/// <inheritdoc cref="BootstrapFormViewer{Model}"/>
public sealed partial class BootstrapFormViewerProperty<Model> : ComponentBase
    where Model : class
{
    #region 组件参数
    #region 渲染参数
    /// <summary>
    /// 获取渲染这个属性的参数
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFormViewerPropertyInfo<Model> RenderPropertyInfo { get; set; }
    #endregion
    #region 级联参数：用来进行递归渲染的委托
    /// <summary>
    /// 获取用来递归渲染属性的委托
    /// </summary>
    [CascadingParameter]
    private RenderFragment<RenderFormViewerPropertyInfoRecursion>? RenderRecursion { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 当上传时触发的事件
    /// <summary>
    /// 当上传文件时，触发这个事件
    /// </summary>
    /// <param name="info">用来上传文件的参数</param>
    /// <param name="files">所有文件，它会被写入模型的属性</param>
    /// <returns></returns>
    private Task OnUpload(UploadTaskInfo info, IReadOnlyList<IHasReadOnlyPreviewFile> files)
    {
        var target = RenderPropertyInfo.FormModel!;
        var hasPreviewFilePropertyDirectInfo = RenderPropertyInfo.HasPreviewFilePropertyInfo.To<IHasPreviewFilePropertyDirectInfo>()!;
        hasPreviewFilePropertyDirectInfo.SetPreviewFile(target, files);
        return Task.CompletedTask;
    }
    #endregion
    #endregion
}
