using System.Text.Json;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 如果得到授权，
/// 这个组件可以将网络摄像头的画面显示在一个video标签中
/// </summary>
public partial class Monitor : ComponentBase, IDisposable
{
    #region 组件参数
    #region 授权成功时调用的委托
    /// <summary>
    /// 当摄像头权限授权成功时，调用的委托
    /// </summary>
    [Parameter]
    public Func<Task>? AuthorizationSucceeded { get; set; }
    #endregion
    #region 授权失败时调用的委托
    /// <summary>
    /// 当摄像头授权失败时，调用的委托
    /// </summary>
    [Parameter]
    public Func<Task>? AuthorizationFailed { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 这个属性是video标签的参数展开
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? VideoAttributes { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region video标签的ID
    /// <summary>
    /// 获取video标签的ID，
    /// 它可以用来找到这个标签
    /// </summary>
    public string VideoID { get; } = Guid.NewGuid().ToString();
    #endregion
    #region 释放对象
    public void Dispose()
    {
        AuthorizationFailedPack?.Dispose();
        AuthorizationFailedPack?.Dispose();
        GC.SuppressFinalize(this);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 对授权成功方法的封装
    /// <summary>
    /// 对授权成功时执行的方法的JS封装，
    /// 它应该被及时释放
    /// </summary>
    private IDisposable? AuthorizationSucceededPack { get; set; }
    #endregion
    #region 对授权失败方法的封装
    /// <summary>
    /// 对授权失败时执行的方法的JS封装，
    /// 它应该被及时释放
    /// </summary>
    private IDisposable? AuthorizationFailedPack { get; set; }
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;
        (var authorizationSucceeded, AuthorizationSucceededPack) = await JSWindow.Document.PackNetMethod<JsonElement>(_ => AuthorizationSucceeded?.Invoke());
        (var authorizationFailed, AuthorizationFailedPack) = await JSWindow.Document.PackNetMethod<JsonElement>(_ => AuthorizationFailed?.Invoke());
        var script = $$$"""
        var element=document.getElementById('{{{VideoID}}}');
        var constraints={ audio: true, video: true,
        width: element.width,
        height: element.height};
        navigator.mediaDevices.getUserMedia(constraints)
        .then(function(stream) 
        {
            var CompatibleURL = window.URL || window.webkitURL;
            try
            {
                element.src = CompatibleURL.createObjectURL(stream);
            }catch(e)
            {
                element.srcObject = stream;
            }
            element.play();
            {{{authorizationSucceeded}}}();
        })
        .catch(function(err) 
        {
            {{{authorizationFailed}}}();
        });
        """;
        await JSWindow.InvokeCodeVoidAsync(script);
    }
    #endregion
    #endregion
}