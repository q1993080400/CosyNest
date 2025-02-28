namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个转接播放器，
/// 它将原始Uri转换为Blob形式的Uri，
/// 这可以用来规避某些国产浏览器对video标签的劫持
/// </summary>
public sealed partial class TransferVideo : ComponentBase, IAsyncDisposable
{
    #region 组件参数
    #region 用来渲染组件的委托
    /// <summary>
    /// 获取用来渲染整个组件的委托，
    /// 它的参数就是经过转换，Blob形式的Uri
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<string> RenderComponent { get; set; }
    #endregion
    #region 原始Uri
    /// <summary>
    /// 获取原始的Uri，
    /// 它未经转换
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Uri { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region 释放对象
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (BlobUri is { })
                await JSWindow.InvokeVoidAsync("DisposableObjectURL", [BlobUri]);
        }
        catch (JSDisconnectedException)
        {
        }
    }
    #endregion
    #endregion
    #region 内部成员
    #region 经过转换的Uri
    /// <summary>
    /// 经过转换，Blob形式的Uri
    /// </summary>
    private string? BlobUri { get; set; }
    #endregion
    #region 内部成员
    #region 重写OnInitializedAsync方法
    protected override async Task OnInitializedAsync()
    {
        BlobUri = await JSWindow.InvokeAsync<string>("ToBlobUri", Uri);
    }
    #endregion
    #endregion
    #endregion
}
