namespace Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// 该组件可以用于上传文件，
/// 它在UI上不显示，上传对话框的样式由其他组件提供
/// </summary>
public sealed partial class FileUpload : ComponentBase
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
    #region 参数展开
    /// <summary>
    /// 这个字典为上传组件提供参数展开
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region 选择文件
    /// <summary>
    /// 调用本方法以选择文件
    /// </summary>
    public async Task SelectFile()
    {
        await JSWindow.InvokeCodeVoidAsync($"document.getElementById('{ID}').click()");
    }
    #endregion
    #endregion
    #region 内部成员
    #region 关于初始化
    #region 选择器ID
    /// <summary>
    /// 获取选择器的ID
    /// </summary>
    private string ID { get; } = ToolASP.CreateJSObjectName();
    #endregion
    #region 重写SetParametersAsync
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var additionalAttributes = parameters.ToDictionary().ToDictionary(true);
        additionalAttributes["id"] = ID;
        await base.SetParametersAsync(ParameterView.FromDictionary(additionalAttributes!));
    }
    #endregion
    #endregion
    #region 关于选择文件
    #region 选择文件时执行的方法
    /// <summary>
    /// 选择文件时执行的方法
    /// </summary>
    /// <param name="args">用来描述被选择文件的对象</param>
    private async void OnChange(InputFileChangeEventArgs args)
    {
        var files = args.GetMultipleFiles(args.FileCount);
        await OnSelectFile(files);
    }
    #endregion
    #endregion
    #endregion
}
