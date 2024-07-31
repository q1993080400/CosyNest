using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是使用Bootstrap实现的文件上传组件
/// </summary>
public sealed partial class BootstrapFileUpload : ComponentBase, IContentComponent<RenderFragment<bool>>
{
    #region 组件参数
    #region 用来上传文件的委托
    /// <inheritdoc cref="FileUpload.OnSelectFile"/>
    [Parameter]
    [EditorRequired]
    public Func<OnSelectFileEventArgs, Task<bool>> OnSelectFile { get; set; }
    #endregion
    #region 上传状态改变时的委托
    /// <inheritdoc cref="FileUpload.OnUploadStatusChange"/>
    [Parameter]
    [EditorRequired]
    public Func<FileUploadStatus, Task> OnUploadStatusChange { get; set; }
    #endregion
    #region 是否可多选
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示允许多选文件
    /// </summary>
    [Parameter]
    public bool Multiple { get; set; }
    #endregion
    #region 限制可选文件类型
    /// <summary>
    /// 这个字符串可以限制可选的文件类型，
    /// 它的语法和Web中input标签的Accept属性相同
    /// </summary>
    [Parameter]
    public string? Accept { get; set; }
    #endregion
    #region 子内容
    /// <summary>
    /// 用来渲染文件上传器可见部分的委托，
    /// 它的参数就是是否正在上传文件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<bool> ChildContent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 组件ID
    /// <summary>
    /// 获取这个组件的ID
    /// </summary>
    private string ID { get; } = CreateASP.JSObjectName();
    #endregion
    #endregion
}
