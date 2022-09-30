namespace Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// 该组件可以用于上传文件，
/// 它在UI上不显示，上传对话框的样式由其他组件提供
/// </summary>
public sealed partial class FileUpload : ComponentBase
{
    #region 关于初始化
    #region 选择器ID
    /// <summary>
    /// 获取选择器的ID
    /// </summary>
    private string ID { get; } = Guid.NewGuid().ToString();
    #endregion
    #region 参数展开
    /// <summary>
    /// 获取应用到input元素的附加属性的集合
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }
    #endregion
    #region 捕获文件选择器
    /// <summary>
    /// 捕获的文件选择器
    /// </summary>
    private InputFile? Capture { get; set; }
    #endregion
    #endregion 
    #region 重写SetParametersAsync
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        (AdditionalAttributes ??= new Dictionary<string, object>())["id"] = ID;
        AdditionalAttributes["style"] = "display:none;" + AdditionalAttributes.TryGetValue("style").Value;
    }
    #endregion
    #region 关于选择文件
    #region 选择文件时执行的方法
    /// <summary>
    /// 选择文件时执行的方法
    /// </summary>
    /// <param name="args">用来描述被选择文件的对象</param>
    private void OnChange(InputFileChangeEventArgs args)
    {
        var files = args.GetMultipleFiles(args.FileCount);
        OnSelectFile?.Invoke(files);
    }
    #endregion
    #region 选择文件时触发的事件
    /// <summary>
    /// 获取或设置选择文件时触发的事件
    /// </summary>
    public Action<IEnumerable<IBrowserFile>>? OnSelectFile { get; set; }
    #endregion
    #region 选择文件
    /// <summary>
    /// 调用本方法以选择文件
    /// </summary>
    public async Task SelectFile()
    {
        var element = (await JSWindow.Document.GetElementById(ID)) ??
            throw new KeyNotFoundException($"未找到ID为{ID}的元素");
        await element.Click();
    }
    #endregion 
    #endregion
}
