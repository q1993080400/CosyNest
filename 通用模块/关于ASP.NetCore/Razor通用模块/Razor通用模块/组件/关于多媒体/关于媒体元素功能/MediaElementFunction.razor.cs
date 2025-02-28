namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件允许为它的后代媒体元素添加一些功能
/// </summary>
public sealed partial class MediaElementFunction : ComponentBase
{
    #region 组件参数
    #region 用来渲染整个组件的委托
    /// <summary>
    /// 获取用来渲染整个组件的委托，
    /// 它的参数是一个ID，
    /// 必须把这个ID赋值给容纳子内容的容器元素
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<string> RenderComponent { get; set; }
    #endregion
    #region 组件ID
    /// <summary>
    /// 获取组件的ID，必须将它赋值给父容器元素，
    /// 否则本组件无法发挥功能
    /// </summary>
    [Parameter]
    public string ID { get; set; } = CreateASP.JSObjectName();
    #endregion
    #region 是否看到媒体时自动播放
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则启用看到媒体时自动播放，
    /// 看不到的时候自动暂停的功能
    /// </summary>
    [Parameter]
    public bool VisiblePlay { get; set; }
    #endregion
    #region 是否全局音量
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则启用全局音量功能，所有媒体元素会共享同样的音量大小，
    /// 并且会记住这个音量
    /// </summary>
    [Parameter]
    public bool GlobalVolume { get; set; }
    #endregion
    #region 是否唯一音量
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则父容器范围内只允许唯一一个拥有<see cref="OnlyVolumeAttribute"/>特性的媒体元素播放声音，
    /// 其他媒体元素会被全部静音
    /// </summary>
    [Parameter]
    public bool OnlyVolume { get; set; }
    #endregion
    #endregion
    #region 静态成员
    #region 唯一音量特性
    /// <summary>
    /// 获取一个html标签特性的名称，
    /// 在<see cref="OnlyVolume"/>为<see langword="true"/>的情况下，
    /// 只有具有这个特性的媒体元素才会播放声音
    /// </summary>
    public const string OnlyVolumeAttribute = "onlyvolume";
    #endregion
    #region 只播放可见视频声音特性
    /// <summary>
    /// 获取一个html标签特性的名称，
    /// 在<see cref="OnlyVolume"/>为<see langword="true"/>的情况下，
    /// 只有可以被用户看见的video才会播放声音，它对audio无效
    /// </summary>
    public const string OnlyVisibleVolumeAttribute = "onlyvisiblevolume";
    #endregion
    #endregion
    #region 内部成员
    #region 重写OnAfterRenderAsync方法
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;
        var options = new
        {
            VisiblePlay,
            GlobalVolume,
            OnlyVolume
        };
        await JSWindow.InvokeVoidAsync("ObserveVisibleMediaElement", ID, options);
    }
    #endregion
    #endregion
}
