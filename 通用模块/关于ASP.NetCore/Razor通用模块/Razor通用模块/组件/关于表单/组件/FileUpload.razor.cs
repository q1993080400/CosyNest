namespace Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// 该组件可以用于上传文件，
/// 它在UI上不显示，上传对话框的样式由其他组件提供
/// </summary>
public sealed partial class FileUpload : ComponentBase, IContentComponent<RenderFragment>
{
    #region 组件参数
    #region 选择文件时触发的事件
    /// <summary>
    /// 获取或设置选择文件时触发的事件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<IReadOnlyList<IBrowserFile>, Task> OnSelectFile { get; set; }
    #endregion
    #region 子内容
    /// <summary>
    /// 获取组件的子内容，
    /// 它允许自定义组件的样式
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment? ChildContent { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 这个字典为上传组件提供参数展开
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 关于选择文件
    #region 选择文件时执行的方法
    /// <summary>
    /// 选择文件时执行的方法
    /// </summary>
    /// <param name="args">用来描述被选择文件的对象</param>
    private async Task OnChange(InputFileChangeEventArgs args)
    {
        var files = args.GetMultipleFiles(args.FileCount);
        await OnSelectFile(files);
    }
    #endregion
    #endregion
    #endregion
}
