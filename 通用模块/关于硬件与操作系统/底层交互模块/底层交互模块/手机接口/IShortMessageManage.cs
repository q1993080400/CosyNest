namespace System.Underlying.Phone;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来管理短信
/// </summary>
public interface IShortMessageManage
{
    #region 发送短信
    /// <summary>
    /// 发送短信
    /// </summary>
    /// <param name="message">短信消息</param>
    /// <param name="addressee">短信收件人</param>
    /// <param name="cancellation">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task Send(string message, IEnumerable<string> addressee, CancellationToken cancellation = default);
    #endregion
}
