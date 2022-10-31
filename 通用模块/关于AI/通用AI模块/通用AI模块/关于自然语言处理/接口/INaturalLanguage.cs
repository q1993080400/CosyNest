namespace System.AI.NaturalLanguage;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来执行自然语言处理
/// </summary>
public interface INaturalLanguage
{
    #region 人机对话
    /// <summary>
    /// 向程序询问一个问题，并得到答案
    /// </summary>
    /// <param name="problem">要询问的问题</param>
    /// <returns></returns>
    Task<string> Dialogue(string problem);
    #endregion
}
