namespace System.AI;

/// <summary>
/// 这个记录表示AI聊天中的一条消息
/// </summary>
public sealed record AIChatMessage
{
    #region 是否为人类的消息
    /// <summary>
    /// 获取这条消息是否为人类说出的
    /// </summary>
    public required bool IsHuman { get; init; }
    #endregion
    #region 消息正文
    /// <summary>
    /// 获取消息的正文
    /// </summary>
    public required string Message { get; init; }
    #endregion
}
