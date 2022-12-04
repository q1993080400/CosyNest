namespace System;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个消息封包，
/// 它封装了消息传递的状态，以及传递的消息
/// </summary>
/// <typeparam name="StatusCode">消息状态码的类型</typeparam>
/// <typeparam name="Message">消息的类型</typeparam>
public interface IMessagePacket<StatusCode, Message>
{
    #region 消息的状态
    /// <summary>
    /// 获取消息传递的状态码
    /// </summary>
    StatusCode Status { get; }
    #endregion
    #region 消息传递是否成功
    /// <summary>
    /// 获取消息传递是否成功
    /// </summary>
    bool IsSuccess { get; }
    #endregion
    #region 消息的内容
    /// <summary>
    /// 获取消息的内容
    /// </summary>
    Message Content { get; }
    #endregion
}
