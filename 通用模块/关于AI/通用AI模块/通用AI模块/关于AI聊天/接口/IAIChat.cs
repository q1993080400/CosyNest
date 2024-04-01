namespace System.AI;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来执行AI聊天
/// </summary>
public interface IAIChat
{
    #region 创建上下文
    /// <summary>
    /// 创建一个对话上下文，
    /// 它可以进行多轮对话
    /// </summary>
    /// <returns></returns>
    Task<IAIChatContext> Create();
    #endregion
}
