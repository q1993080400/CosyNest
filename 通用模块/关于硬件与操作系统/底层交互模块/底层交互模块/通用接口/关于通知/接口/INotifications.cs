namespace System.Underlying;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来发送通知
/// </summary>
public interface INotifications
{
    #region 发送通知
    /// <summary>
    /// 发送一条通知
    /// </summary>
    /// <param name="options">这个对象描述通知的内容以及配置</param>
    /// <returns></returns>
    Task Send(INotificationsOptions options);
    #endregion
    #region 关闭通知
    /// <summary>
    /// 关闭指定或全部通知
    /// </summary>
    /// <param name="tags">枚举要关闭的通知ID，
    /// 如果为<see langword="null"/>，则关闭所有通知</param>
    /// <returns></returns>
    Task Close(IEnumerable<string>? tags = null);
    #endregion
}
