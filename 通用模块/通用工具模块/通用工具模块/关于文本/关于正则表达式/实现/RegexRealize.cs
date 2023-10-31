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
    #region 匹配所需的信息
    #region 用于匹配的正则表达式
    public string RegexText { get; } = regexText;
    #endregion
    #region 匹配选项
    /// <summary>
    /// 获取用于匹配的选项
    /// </summary>
    private RegexOptions Options { get; } = options;
    #endregion
    #endregion
    #region 关于匹配
    #region 返回是否匹配到了结果
    public bool IsMatch(string? text)
         => text is not null && Regex.IsMatch(text, RegexText, Options);
    #endregion
    #region 返回匹配结果
    public (bool IsMatch, IMatch[] Matches) Matches(string? text)
    {
        if (text is null)
            return (false, Array.Empty<IMatch>());
        var matcher = Regex.Matches(text, RegexText, Options).Select(x => new RegexMatch(x, RegexText)).ToArray();
        return (matcher.Length != 0, matcher);
    }
    #endregion
    #region 返回唯一一个匹配到的结果
    public IMatch? MatcheSingle(string text)
    {
        var matchs = Regex.Matches(text, RegexText, Options);
        return matchs switch
        {
            [] => null,
            [var match] => new RegexMatch(match, RegexText),
            _ => throw new ArgumentException($"""正则表达式"{RegexText}"对文本"{text}"的匹配找到了多个结果""")
        };
    }
    #endregion
    #region 替换匹配到的字符
    public string Replace(string text, Func<string, string> replace)
    {
        foreach (var item in Matches(text).Matches.Select(x => x.Match))
        {
            text = text.Replace(item, replace(item));
        }
        return text;
    }

    #endregion
    #endregion
    #region 构造方法
    #endregion
    #region 静态构造函数
    static RegexRealize()
    {
        Regex.CacheSize = 30;
    }
    #endregion
}
