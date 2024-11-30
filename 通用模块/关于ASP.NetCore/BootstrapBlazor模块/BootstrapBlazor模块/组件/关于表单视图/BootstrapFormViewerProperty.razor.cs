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
    #region 高亮文本
    /// <summary>
    /// 获取高亮文本的集合
    /// </summary>
    [CascadingParameter(Name = SearchPanel.HighlightParameter)]
    private IReadOnlyCollection<string>? Highlight { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 用来上传的方法
    /// <summary>
    /// 当上传文件时，触发这个事件
    /// </summary>
    /// <param name="info">用来描述上传任务的对象</param>
    /// <returns></returns>
    private async Task OnUpload(UploadTaskInfo info)
    {

    }
    #endregion
    #endregion
}
