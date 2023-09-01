namespace System;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个消息封包，
/// 它封装了消息传递的状态，以及传递的消息
/// </summary>
/// <typeparam name="Message">消息的类型</typeparam>
/// <inheritdoc cref="IStatePacket{StatusCode}"/>
public interface IMessagePacket<out StatusCode, out Message> : IStatePacket<StatusCode>
{
    #region 消息的内容
    /// <summary>
    /// 获取消息的内容
    /// </summary>
    Message Content { get; }
    #endregion
}
