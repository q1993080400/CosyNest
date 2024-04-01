namespace System.AI;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个AI聊天的上下文，
/// 同一上下文中的对话会被记忆
/// </summary>
public interface IAIChatContext
{
    #region 历史消息
    /// <summary>
    /// 获取历史消息
    /// </summary>
    IReadOnlyList<AIChatMessage> History { get; }
    #endregion
    #region AI聊天
    /// <summary>
    /// 向程序询问一个问题，并得到答案
    /// </summary>
    /// <param name="problem">要询问的问题</param>
    /// <returns>程序给予的回答</returns>
    Task<string> Dialogue(string problem);
    #endregion
}
