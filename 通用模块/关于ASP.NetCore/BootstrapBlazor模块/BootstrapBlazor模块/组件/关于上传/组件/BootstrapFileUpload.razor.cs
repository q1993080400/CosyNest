using System.DataFrancis;

using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是底层使用Bootstrap实现的上传组件
/// </summary>
public sealed partial class BootstrapFileUpload : ComponentBase
{
    #region 组件参数
    #region 用来上传文件的委托
    /// <summary>
    /// 用来上传文件的委托，
    /// 这个委托的第一个参数是上传参数，
    /// 第二个参数是<see cref="IHasReadOnlyPreviewFile.IsEnable"/>为<see langword="true"/>的所有文件，
    /// 包含<see cref="InitialFiles"/>文件和新上传的文件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<UploadTaskInfo, IReadOnlyList<IHasReadOnlyPreviewFile>, Task> OnUpload { get; set; }
    #endregion
    #region 用来进行上传的参数
    /// <inheritdoc cref="FileUpload.UploadFileOptions"/>
    [Parameter]
    public UploadFileOptions UploadFileOptions { get; set; } = new();
    #endregion
    #region 可接受文件类型
    /// <summary>
    /// 获取上传组件可接受的文件类型，
    /// 它的语法和Web中input标签的accept属性相同
    /// </summary>
    [Parameter]
    public string? Accept { get; set; }
    #endregion
    #region 是否可选择多个文件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示可以选择多个文件，否则仅可选择单个文件
    /// </summary>
    [Parameter]
    public bool Multiple { get; set; }
    #endregion
    #region 初始文件集合
    /// <summary>
    /// 获取初始文件集合，
    /// 它代表以前上传的文件，
    /// 注意：它仅会初始化一次
    /// </summary>
    [Parameter]
    public IEnumerable<IHasReadOnlyPreviewFile> InitialFiles { get; set; } = [];
    #endregion
    #region 上传按钮文字
    /// <summary>
    /// 获取上传按钮的文字
    /// </summary>
    [Parameter]
    public string? UploadButtonText { get; set; }
    #endregion
    #region 用来渲染整个组件的委托
    /// <summary>
    /// 获取用来渲染整个组件的委托
    /// </summary>
    [Parameter]
    public RenderFragment<RenderBootstrapFileUpload>? RenderComponent { get; set; }
    #endregion
    #region 是否正在上传
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示上传正在进行中，你可以给予一些提示，
    /// 提醒用户上传完毕前不要离开
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool InUpload { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 后来上传的文件
    /// <summary>
    /// 获取后来上传的文件
    /// </summary>
    private IEnumerable<IHasReadOnlyPreviewFile> UploadFiles { get; set; } = [];
    #endregion
    #region 要显示的文件
    /// <summary>
    /// 获取最终要显示的文件
    /// </summary>
    private IReadOnlyList<IHasReadOnlyPreviewFile> ShowFiles
    {
        get
        {
            IHasReadOnlyPreviewFile[] files = Multiple ?
                [.. InitialFiles, .. UploadFiles] : [.. UploadFiles];
            var filterFiles = files.Distinct().WhereEnable().ToArray();
            return (Multiple, filterFiles.Length) is (false, > 1) ?
                throw new NotSupportedException($"仅允许上传一个文件，但是上传了多个文件") : filterFiles;
        }
    }
    #endregion
    #region 当上传时触发的方法
    /// <summary>
    /// 当上传文件的时候，触发的方法
    /// </summary>
    /// <param name="info">用来进行上传的参数</param>
    /// <returns></returns>
    private async Task OnUploadFile(UploadTaskInfo info)
    {
        UploadFiles = info.UploadFiles;
        await OnUpload(info, ShowFiles);
        this.StateHasChanged();
    }
    #endregion
    #region 返回渲染参数
    /// <summary>
    /// 返回本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderBootstrapFileUpload GetRenderInfo()
        => new()
        {
            Accept = Accept,
            Files = ShowFiles,
            Multiple = Multiple,
            OnUpload = OnUploadFile,
            UploadFileOptions = UploadFileOptions,
            UploadButtonText = UploadButtonText ?? "上传文件",
            InUpload = InUpload
        };
    #endregion
    #endregion
}
