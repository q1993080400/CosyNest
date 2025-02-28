namespace Microsoft.JSInterop;

/// <summary>
/// 这个记录是Web端发送通知时可选的一个配置选项
/// </summary>
public sealed record WebNotificationsOptions
{
    #region 标题
    /// <summary>
    /// 获取通知标题
    /// </summary>
    public required string Title { get; init; }
    #endregion
    #region 正文
    /// <summary>
    /// 获取通知正文
    /// </summary>
    public required string Body { get; init; }
    #endregion
    #region 通知ID
    /// <summary>
    /// 获取通知ID，
    /// 它用来过滤重复的通知
    /// </summary>
    public string Tag { get; init; } = "3112CF92-EFB2-23E2-6680-57CA65F27AEB";
    #endregion
    #region 图标
    /// <summary>
    /// 获取通知图标的Uri
    /// </summary>
    public string? Icon { get; init; }
    #endregion
    #region 通知跳转到的Uri
    /// <summary>
    /// 获取点击通知后要跳转到的Uri
    /// </summary>
    public string? Uri { get; init; }
    #endregion
    #region 通知长期有效
    /// <summary>
    /// 指示通知是否长期有效，
    /// 除非用户点击，否则不会自动消失
    /// </summary>
    public bool RequireInteraction { get; init; } = true;
    #endregion
    #region 是否仅后台通知
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 而且页面处于前台，则不进行通知
    /// </summary>
    public bool OnlyBackend { get; init; }
    #endregion
    #region 在替换旧通知后，是否重复通知
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则在新通知替换旧通知以后，会再次通知用户
    /// </summary>
    public bool Renotify { get; init; } = true;
    #endregion
}
