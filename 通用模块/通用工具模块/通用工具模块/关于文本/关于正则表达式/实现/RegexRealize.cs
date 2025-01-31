namespace System.Text.RegularExpressions;

/// <summary>
/// 这个类型是<see cref="IRegex"/>的实现，
/// 可以视为一个正则表达式
/// </summary>
/// <remarks>
/// 使用指定的正则表达式初始化对象
/// </remarks>
/// <param name="regexText">指定的正则表达式</param>
/// <param name="options">正则表达式的选项</param>
sealed class RegexRealize(string regexText, RegexOptions options) : IRegex
{
    #region 公开成员
    #region 用于匹配的正则表达式
    public string RegexText
        => regexText;
    #endregion
    #region 关于匹配
    #region 返回是否匹配到了结果
    public bool IsMatch(string? text)
         => text is { } && RegexInstance.IsMatch(text);
    #endregion
    #region 返回匹配结果
    public (bool IsMatch, IMatch[] Matches) Matches(string? text)
    {
        if (text is null)
            return (false, []);
        var matcher = RegexInstance.Matches(text).Select(x => new RegexMatch(x, regexText)).ToArray();
        return (matcher.Length is not 0, matcher);
    }
    #endregion
    #region 返回唯一一个匹配到的结果
    public IMatch? MatcheSingle(string text)
    {
        var matchs = RegexInstance.Matches(text);
        return matchs switch
        {
        [] => null,
        [var match] => new RegexMatch(match, regexText),
            _ => throw new ArgumentException($"""正则表达式"{regexText}"对文本"{text}"的匹配找到了多个结果""")
        };
    }
    #endregion
    #region 替换匹配到的字符
    public string Replace(string text, string replace)
        => RegexInstance.Replace(text, replace);
    #endregion
    #endregion
    #region 拆分字符串
    public string[] Split(string text)
        => RegexInstance.Split(text);
    #endregion
    #endregion
    #region 内部成员
    #region 正则表达式对象
    /// <summary>
    /// 获取封装的正则表达式对象，
    /// 本对象就是通过它实现的
    /// </summary>
    private Regex RegexInstance { get; } = new(regexText, options);
    #endregion
    #endregion
}
