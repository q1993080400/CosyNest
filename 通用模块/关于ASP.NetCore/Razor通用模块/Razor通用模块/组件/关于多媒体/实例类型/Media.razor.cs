namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是更强大的音视频播放器，
/// 它支持更多的功能，例如记忆播放位置和全局音量
/// </summary>
public sealed partial class Media : Component
{
    #region 组件参数
    #region 播放器的名称
    /// <summary>
    /// 获取播放器的名称，
    /// 它决定了用于存储播放器进度的键，
    /// 若不指定，则使用src属性自动生成一个
    /// </summary>
    [Parameter]
    public string MediaName { get; set; }
    #endregion
    #region 是否视频
    /// <summary>
    /// 如果这个播放器是视频，则为<see langword="true"/>，
    /// 是音频，则为<see langword="false"/>
    /// </summary>
    [EditorRequired]
    [Parameter]
    public bool IsVideo { get; set; }
    #endregion
    #region 是否关闭自动记忆播放进度
    /// <summary>
    /// 获取是否关闭自动记忆播放进度，它可以节约性能，
    /// 而且对于类似短视频的应用，不需要这个功能
    /// </summary>
    [Parameter]
    public bool CloseMemoryProgress { get; set; }
    #endregion
    #region 播放器的ID
    /// <summary>
    /// 获取播放器组件的ID，
    /// 如果不指定，则程序会为你自动指定一个
    /// </summary>
    [Parameter]
    public string ID { get; set; } = ToolASP.CreateJSObjectName();
    #endregion
    #region 触发OnLoadedMetaData事件时执行的脚本
    /// <summary>
    /// 当触发OnLoadedMetaData事件时，执行这个脚本，
    /// 如果为<see langword="null"/>，则不执行
    /// </summary>
    [Parameter]
    public string? OnLoadedMetaData { get; set; }
    #endregion
    #region 触发OnTimeupDate事件时执行的脚本
    /// <summary>
    /// 当触发OnTimeupDate事件时，执行这个脚本，
    /// 如果为<see langword="null"/>，则不执行
    /// </summary>
    [Parameter]
    public string? OnTimeUpDate { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 该参数展开控制video标签的样式
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? MediaAttributes { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 用来提取音量的键
    /// <summary>
    /// 这个键用来从本地存储中提取音量
    /// </summary>
    private const string VolumeKey = "Volume";
    #endregion
    #region OnEnded事件触发时执行的方法
    /// <summary>
    /// 当OnEnded事件触发时，执行的方法，
    /// 它在音视频播放结束后，将记忆的播放位置清除
    /// </summary>
    /// <returns></returns>
    private async Task OnEndedEvent()
    {
        if (!CloseMemoryProgress)
            await JSWindow.LocalStorage.RemoveAsync(MediaName);
    }
    #endregion
    #region OnLoadedMetaData事件触发时执行的方法
    /// <summary>
    /// 当OnLoadedMetaData事件触发时，执行的方法，
    /// 它在加载音视频时，自动跳转到记忆的播放位置
    /// </summary>
    /// <returns></returns>
    private async Task OnLoadedMetaDataEvent()
    {
        var element = (await JSWindow.Document.GetElementById(ID))!;
        if (!CloseMemoryProgress)
        {
            var (_, currentTime) = await JSWindow.LocalStorage.TryGetValueAsync(MediaName);
            if (currentTime is { })
                await element.Index.Set("currentTime", currentTime);
        }
        var (_, volume) = await JSWindow.LocalStorage.TryGetValueAsync(VolumeKey);
        await element.Index.Set("volume", volume ?? "0.6");
        if (OnLoadedMetaData is { })
            await JSWindow.InvokeCodeVoidAsync(OnLoadedMetaData);
    }
    #endregion
    #region OnTimeupDate事件触发时执行的脚本
    /// <summary>
    /// OnTimeupDate事件触发时，执行的脚本，
    /// 它在播放音视频的时候，更新记忆的播放位置
    /// </summary>
    private string OnTimeUpDateEvent()
    {
        var script = CloseMemoryProgress ? "" : $$"""
            var element=document.getElementById('{{ID}}');
            if(element!=null)
            {
                var currentTime=element.currentTime;
                localStorage.setItem('{{MediaName}}',currentTime.toString());
            }
            """;
        if (OnTimeUpDate is { })
        {
            var jsObjectName = ToolASP.CreateJSObjectName();
            script += $$"""
                function {{jsObjectName}}()
                {
                    {{OnTimeUpDate}}
                };
                {{jsObjectName}}();
                """;
        }
        return script;
    }
    #endregion
    #region OnVolumeChange事件触发时执行的方法
    /// <summary>
    /// OnVolumeChange事件触发时，执行的方法，
    /// 它将音量写入本地存储
    /// </summary>
    /// <returns></returns>
    private async Task OnVolumeChangeEvent()
    {
        var element = (await JSWindow.Document.GetElementById(ID))!;
        var volume = await element.Index.Get("volume");
        await JSWindow.LocalStorage.IndexAsync.Set(VolumeKey, volume);
    }
    #endregion
    #region 重写OnParametersSet
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (MediaName is null && (MediaAttributes?.TryGetValue("src", out var src) ?? false))
        {
            MediaName = $"Media-{src}";
        }
    }
    #endregion 
    #endregion
}
