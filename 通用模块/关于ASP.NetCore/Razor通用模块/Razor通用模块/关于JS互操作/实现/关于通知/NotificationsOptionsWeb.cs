using System.Underlying;

namespace Microsoft.JSInterop;

/// <summary>
/// 这个记录是Web端发送通知时可选的一个配置选项
/// </summary>
public sealed record NotificationsOptionsWeb : INotificationsOptions
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
    public string? Tag { get; init; }
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
}
