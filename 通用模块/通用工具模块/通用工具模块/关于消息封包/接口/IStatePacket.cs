namespace System;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个状态封包，
/// 它可以指示一个操作的状态
/// </summary>
/// <typeparam name="StatusCode">消息状态码的类型</typeparam>
public interface IStatePacket<out StatusCode>
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
}
