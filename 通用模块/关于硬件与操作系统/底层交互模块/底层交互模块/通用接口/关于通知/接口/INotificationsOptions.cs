namespace System.Underlying;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个发送通知时的选项
/// </summary>
public interface INotificationsOptions
{
    #region 标题
    /// <summary>
    /// 获取通知标题
    /// </summary>
    string Title { get; }
    #endregion
    #region 正文
    /// <summary>
    /// 获取通知正文
    /// </summary>
    string Body { get; }
    #endregion
    #region 通知ID
    /// <summary>
    /// 获取通知ID，
    /// 它用来过滤重复的通知
    /// </summary>
    string? Tag { get; }
    #endregion
    #region 图标
    /// <summary>
    /// 获取通知图标的Uri
    /// </summary>
    string? Icon { get; }
    #endregion
    #region 通知长期有效
    /// <summary>
    /// 指示通知是否长期有效，
    /// 除非用户点击，否则不会自动消失
    /// </summary>
    bool RequireInteraction { get; }
    #endregion
}