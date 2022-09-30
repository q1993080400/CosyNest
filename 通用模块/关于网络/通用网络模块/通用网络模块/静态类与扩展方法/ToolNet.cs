using System.Text.RegularExpressions;

namespace System.NetFrancis;

/// <summary>
/// 关于网络的工具类
/// </summary>
public static class ToolNet
{
    #region 关于Uri
    #region 提取Uri中的参数
    #region 辅助属性
    /// <summary>
    /// 获取用来匹配Uri参数的正则表达式
    /// </summary>
    private static IRegex MatchParameter { get; }
    = /*language=regex*/@"[\?&](?<name>[^=]+)=(?<value>[^&]+)".Op().Regex();
    #endregion
    #region 正式方法
    /// <summary>
    /// 提取一个Uri中的参数（如果有）
    /// </summary>
    /// <param name="uri">待提取参数的Uri</param>
    /// <returns>一个元组，它的项分别是Uri是否含有参数，
    /// Uri的非参数部分，以及枚举Uri参数部分的字典（如果没有参数，则为空字典）</returns>
    public static (bool HasParameters, string Uri, IReadOnlyDictionary<string, string> Parameters) ExtractionParameters(string uri)
    {
        var (isMatch, matches) = MatchParameter.Matches(uri);
        if (!isMatch)
            return (false, uri, CreateCollection.EmptyDictionary<string, string>());
        var par = matches.Select(x => (x["name"].Match, x["value"].Match)).ToDictionary(true);
        return (true, uri.Split("?")[0], par);
    }
    #endregion
    #endregion
    #endregion
}
