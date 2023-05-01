namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是<see cref="Media"/>组件的渲染参数
/// </summary>
public sealed record RenderMedia
{
    #region 播放器的ID
    /// <summary>
    /// 获取播放器标签的ID，
    /// 为了使本类型正确地发挥作用，
    /// 必须将这个ID赋值给播放器
    /// </summary>
    public required string ID { get; init; }
    #endregion
    #region OnEnded事件触发时执行的脚本
    /// <inheritdoc cref="Media.OnEndedScript"/>
    public required string OnEndedScript { get; init; }
    #endregion
    #region OnLoadedMetaData事件触发时执行的脚本
    /// <inheritdoc cref="Media.OnLoadedMetaDataScript"/>
    public required string OnLoadedMetaDataScript { get; init; }
    #endregion
    #region OnTimeupDate事件触发时执行的脚本
    /// <inheritdoc cref="Media.OnTimeUpDateScript"/>
    public required string OnTimeUpDateScript { get; init; }
    #endregion
    #region OnVolumeChange事件触发时执行的脚本
    /// <inheritdoc cref="Media.OnVolumeChangeScript"/>
    public required string OnVolumeChangeScript { get; init; }
    #endregion
    #region 在脚本外追加新的脚本
    /// <param name="appendScript">用来返回新脚本的委托，它的参数就是播放器的ID</param>
    /// <inheritdoc cref="ToolRazor.AppendScript(string, string)"/>
    public string AppendScript(string script, Func<string, string> appendScript)
        => ToolRazor.AppendScript(script, appendScript(ID));
    #endregion
}
