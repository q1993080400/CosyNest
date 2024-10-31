namespace System.Text.RegularExpressions;

/// <summary>
/// 这个静态类储存了一些常用的正则表达式
/// </summary>
public static class CommonlyRegex
{
    #region 匹配十六进制数
    /// <summary>
    /// 返回用于匹配16进制数的正则表达式，
    /// 注意：按照规范，A-F须为大写
    /// </summary>
    public static IRegex Sys16 { get; }
        =/*language=regex*/ @"[0-9A-Fa-f]".Op().Regex();
    #endregion
    #region 匹配数字
    /// <summary>
    /// 返回匹配数字的正则表达式，
    /// 包含小数
    /// </summary>
    public static IRegex Num { get; }
        =/*language=regex*/@"\d+(\.\d+)?".Op().Regex();
    #endregion
    #region 匹配数字或字母
    /// <summary>
    /// 返回匹配正整数或者字母的正则表达式
    /// </summary>
    public static IRegex IntOrLetters { get; }
        = /*language=regex*/@"[0-9a-zA-Z]".Op().Regex();
    #endregion
    #region 匹配中文汉字
    /// <summary>
    /// 返回用于匹配中文汉字的正则表达式
    /// </summary>
    public static IRegex Chinese { get; }
        = /*language=regex*/@"[\u4e00-\u9fa5]".Op().Regex();
    #endregion
    #region 匹配键值对
    #region 返回表达式
    /// <summary>
    /// 返回用于匹配键值对的正则表达式
    /// </summary>
    /// <param name="separator">键值对之间的分隔符，
    /// 如果存在多个分隔符，则将它们全部塞在这个字符串中，中间不要有空格</param>
    /// <param name="separatorKey">键与值之间的分隔符</param>
    /// <returns></returns>
    public static IRegex KeyValuePair(string separator, string separatorKey = "=")
        => @$"(?<key>[^{separator}{separatorKey}]+){separatorKey}(?<value>[^{separator}]+)".Op().Regex();
    #endregion
    #region 提取键值对
    /// <summary>
    /// 通过正则表达式，从字符串中提取键值对
    /// </summary>
    /// <param name="text">要提取键值对的字符串</param>
    /// <returns></returns>
    /// <inheritdoc cref="KeyValuePair(string, string)"/>
    public static IReadOnlyDictionary<string, string> KeyValuePairExtraction(string text, string separator, string separatorKey = "=")
        => KeyValuePair(separator, separatorKey).Matches(text).Matches.
        ToDictionary(x => x["key"].Match, x => x["value"].Match);
    #endregion
    #endregion
}
