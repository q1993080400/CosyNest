namespace System.Text.RegularExpressions;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个正则表达式
/// </summary>
public interface IRegex
{
    #region 用于匹配的正则表达式
    /// <summary>
    /// 获取用于匹配的正则表达式
    /// </summary>
    string RegexText { get; }
    #endregion
    #region 关于匹配结果
    #region 返回是否匹配到了结果
    /// <summary>
    /// 返回在指定的文本中是否匹配到了结果
    /// </summary>
    /// <param name="text">待匹配的文本，
    /// 如果为<see langword="null"/>，则直接返回<see langword="false"/></param>
    /// <returns></returns>
    bool IsMatch(string? text)
        => Matches(text).IsMatch;
    #endregion
    #region 返回匹配结果
    /// <summary>
    /// 返回指定文本的匹配结果
    /// </summary>
    /// <param name="text">待匹配的文本，
    /// 如果为<see langword="null"/>，则视为不匹配</param>
    /// <returns>一个元组，它的项分别是是否匹配到了结果，
    /// 以及匹配的结果（如果没有任何匹配，则返回空数组）</returns>
    (bool IsMatch, IMatch[] Matches) Matches(string? text);
    #endregion
    #region 返回唯一匹配到的结果
    /// <summary>
    /// 返回唯一一个匹配到的结果，
    /// 如果没有任何匹配，返回<see langword="null"/>，
    /// 如果存在多个匹配，则引发异常
    /// </summary>
    /// <param name="text">待匹配的文本</param>
    /// <returns></returns>
    IMatch? MatcheSingle(string text);
    #endregion
    #endregion
    #region 关于替换与删除
    #region 替换匹配到的字符
    /// <summary>
    /// 替换所有匹配到的字符，
    /// 并返回替换后的文本
    /// </summary>
    /// <param name="text">待修改的原始文本</param>
    /// <param name="replace">该委托的参数是需要替换的原始文本，
    /// 返回值是用来代替的新文本</param>
    /// <returns></returns>
    string Replace(string text, Func<string, string> replace);
    #endregion
    #region 删除匹配到的字符
    /// <summary>
    /// 删除匹配到的所有字符
    /// </summary>
    /// <param name="text">待修改的原始文本</param>
    /// <returns></returns>
    string Remove(string text)
        => Replace(text, _ => "");
    #endregion
    #endregion
}
