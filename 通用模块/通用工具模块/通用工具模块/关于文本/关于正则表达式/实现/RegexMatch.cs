using System.Collections.Immutable;

namespace System.Text.RegularExpressions;

/// <summary>
/// 这个结构是<see cref="IMatch"/>的实现，
/// 可以用来表示一个正则表达式匹配结果
/// </summary>
sealed partial class RegexMatch : IMatch
{
    #region 关于匹配
    #region 匹配到的字符
    public string Match { get; }
    #endregion
    #region 匹配到的组
    public IReadOnlyList<IMatch> Groups { get; }
    #endregion
    #region 获取匹配组的名称
    public string? Name { get; }
    #endregion
    #region 获取命名匹配组
    public IReadOnlyDictionary<string, IMatch> GroupsNamed { get; }
    #endregion
    #region 发现字符串的索引
    public int Index { get; }
    #endregion
    #region 字符串的长度
    public int Length { get; }
    #endregion
    #endregion
    #region 重写的ToString方法
    public override string ToString()
        => Match;
    #endregion
    #region 构造函数
    #region 指定匹配字符和组
    /// <summary>
    /// 使用指定的匹配字符和组初始化对象
    /// </summary>
    /// <param name="match">匹配到的字符</param>
    /// <param name="index">发现这个字符的索引</param>
    /// <param name="length">字符的长度</param>
    private RegexMatch(string match, int index, int length)
    {
        Match = match;
        Groups = [];
        GroupsNamed = ImmutableDictionary<string, IMatch>.Empty;
        Index = index;
        Length = length;
    }
    #endregion
    #region 指定匹配结果
    /// <summary>
    /// 使用指定的匹配结果初始化对象
    /// </summary>
    /// <param name="match">指定的匹配结果</param>
    /// <param name="regular">用来匹配的正则表达式</param>
    public RegexMatch(Capture match, string regular)
    {
        Match = match.Value;
        Name = (match as Group)?.Name;
        Index = match.Index;
        Length = match.Length;
        Groups = [.. (match switch
        {
            Match m => m.Groups.OfType<Group>().Skip(1).Where(x => x.Success),
            Group m => (IEnumerable<Capture>)m.Captures,
            _ => []
        }).
        Select(x => x == match ? new RegexMatch(x.Value, x.Index, x.Length) : (IMatch)new RegexMatch(x, regular))];
        var names = MatchGroupName().Matches(regular).Select(x => x.Value[3..^1]).ToHashSet();
        GroupsNamed = Groups.Where(x => x.Name != null && names.Contains(x.Name)).ToDictionary(x => x.Name!, x => x);
    }
    #endregion
    #region 辅助方法：预编译正则表达式
    /// <summary>
    /// 这个方法可用于预编译正则表达式，提高性能
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex("\\(\\?\\<\\S+?\\>")]
    private static partial Regex MatchGroupName();
    #endregion
    #endregion
}
