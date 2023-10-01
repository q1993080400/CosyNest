namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是更强大的音视频播放器，
/// 它支持更多的功能，例如记忆播放位置和全局音量
/// </summary>
public sealed partial class Media : Component, IContentComponent<RenderFragment<RenderMedia>>
{
    #region 组件参数
    #region 是否关闭自动记忆播放进度
    /// <summary>
    /// 获取是否关闭自动记忆播放进度，它可以节约性能，
    /// 而且对于类似短视频的应用，不需要这个功能
    /// </summary>
    [Parameter]
    public bool CloseMemoryProgress { get; set; }
    #endregion
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderMedia> ChildContent { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region 用来提取音量的键
    /// <summary>
    /// 这个键用来从本地存储中提取音量
    /// </summary>
    public const string VolumeKey = "Volume";
    #endregion
    #endregion
    #region 内部成员
    #region 播放器的ID
    /// <summary>
    /// 获取播放器组件的ID
    /// </summary>
    private string ID { get; } = ToolASP.CreateJSObjectName();
    #endregion
    #region OnEnded事件触发时执行的脚本
    /// <summary>
    /// 当OnEnded事件触发时，执行这个脚本，
    /// 它在播放完毕后，将播放进度移除
    /// </summary>
    private string OnEndedScript
        => CloseMemoryProgress ? "" :
        $$"""
        var element=document.getElementById('{{ID}}');
        var key='Media-'+element.src;
        localStorage.removeItem(key);
        """;
    #endregion
    #region OnLoadedMetaData事件触发时执行的脚本
    /// <summary>
    /// 当OnLoadedMetaData事件触发时，执行的脚本，
    /// 它在加载音视频时，自动跳转到记忆的播放位置
    /// </summary>
    private string OnLoadedMetaDataScript
    {
        get
        {
            var script = $"var element=document.getElementById('{ID}');";
            if (!CloseMemoryProgress)
                script += $$"""
                    var key='Media-'+element.src;
                    var currentTime=localStorage.getItem(key);
                    if(currentTime!=null)
                        element.currentTime=currentTime;
                    """;
            script += $$"""
                var volume=localStorage.getItem('{{VolumeKey}}');
                element.volume=volume??0.6;
                """;
            return script;
        }
    }
    #endregion
    #region OnTimeupDate事件触发时执行的脚本
    /// <summary>
    /// OnTimeupDate事件触发时，执行的脚本，
    /// 它在播放音视频的时候，更新记忆的播放位置
    /// </summary>
    private string OnTimeUpDateScript
    {
        get
        {
            var script = CloseMemoryProgress ? "" : $$"""
            var element=document.getElementById('{{ID}}');
            var key='Media-'+element.src;
            var currentTime=element.currentTime;
            localStorage.setItem(key,currentTime.toString());
            """;
            return script;
        }
    }
    #endregion
    #region OnVolumeChange事件触发时执行的脚本
    /// <summary>
    /// OnVolumeChange事件触发时，执行的脚本，
    /// 它在音量被改变的时候，将改变后的音量写入本地存储
    /// </summary>
    private string OnVolumeChangeScript
        => $$"""
        var element=document.getElementById('{{ID}}');
        var volume =element.volume;
        localStorage.setItem('{{VolumeKey}}',volume.toString());
        """;
    #endregion
    #region 获取用来渲染组件的参数
    /// <summary>
    /// 获取用来渲染组件的参数
    /// </summary>
    /// <returns></returns>
    private RenderMedia GetRenderInfo()
        => new()
        {
            ID = ID,
            OnEndedScript = OnEndedScript,
            OnLoadedMetaDataScript = OnLoadedMetaDataScript,
            OnTimeUpDateScript = OnTimeUpDateScript,
            OnVolumeChangeScript = OnVolumeChangeScript,
        };
    #endregion
    #endregion
}
