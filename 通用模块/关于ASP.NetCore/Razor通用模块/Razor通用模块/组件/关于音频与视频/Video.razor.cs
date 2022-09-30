namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是更强大的视频播放器，
/// 它支持更多的功能，例如记忆播放位置
/// </summary>
public sealed partial class Video : Component
{
    #region 组件参数
    #region 视频的名称
    /// <summary>
    /// 获取视频的名称，
    /// 它决定了用于存储视频进度的键，
    /// 若不指定，则使用src
    /// </summary>
    [Parameter]
    public string VideoName { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 该参数展开控制video标签的样式
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? VideoAttributes { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region Video的ID
    /// <summary>
    /// 获取播放器组件的ID
    /// </summary>
    private string ID { get; } = Guid.NewGuid().ToString();
    #endregion
    #region OnEnded事件触发时执行的方法
    /// <summary>
    /// 当OnEnded事件触发时，执行的方法，
    /// 它在视频播放结束后，将记忆的播放位置清除
    /// </summary>
    /// <returns></returns>
    private async Task OnEnded()
        => await JSWindow.LocalStorage.RemoveAsync(VideoName);
    #endregion
    #region OnLoadedMetaData事件触发时执行的方法
    /// <summary>
    /// 当OnLoadedMetaData事件触发时，执行的方法，
    /// 它在加载视频时，自动跳转到记忆的播放位置
    /// </summary>
    /// <returns></returns>
    private async Task OnLoadedMetaData()
    {
        var (exist, currentTime) = await JSWindow.LocalStorage.TryGetValueAsync(VideoName);
        if (!exist)
            return;
        var element = await JSWindow.Document.GetElementById(ID);
        await element!.Index.Set("currentTime", currentTime!);
    }
    #endregion
    #region OnTimeupDate事件触发时执行的方法
    /// <summary>
    /// OnTimeupDate事件触发时，执行的方法，
    /// 它在播放视频的时候，更新记忆的播放位置
    /// </summary>
    private async Task OnTimeupDate()
    {
        var element = await JSWindow.Document.GetElementById(ID);
        var currentTime = await element!.Index.Get("currentTime");
        await JSWindow.LocalStorage.IndexAsync.Set(VideoName, currentTime);
    }
    #endregion
    #region 重写OnParametersSet
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (VideoName is null && (VideoAttributes?.TryGetValue("src", out var src) ?? false))
        {
            VideoName = $"video-{src}";
        }
    }
    #endregion 
    #endregion
}
