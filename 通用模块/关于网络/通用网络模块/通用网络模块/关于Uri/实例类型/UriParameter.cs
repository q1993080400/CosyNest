using System.Text.RegularExpressions;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型表示Uri的参数部分
/// </summary>
public sealed record UriParameter : UriBase
{
    #region 隐式类型转换
    public static implicit operator UriParameter(string uri)
        => new(uri);
    #endregion
    #region 公开成员
    #region 参数部分文本
    public override string Text
        => Parameter.Join(x => $"{x.Key}={x.Value}", "&");
    #endregion
    #region 参数字典
    /// <summary>
    /// 获取按照参数名称枚举参数值的字典
    /// </summary>
    public IReadOnlyDictionary<string, string?> Parameter { get; init; }
    #endregion
    #endregion
    #region 构造函数
    #region 正则表达式
    /// <summary>
    /// 获取用来匹配参数字符串的正则表达式
    /// </summary>
    private static IRegex Regex { get; } =/*language=regex*/@"(?<key>(\w|-|\.)+)=(?<value>([^#& ]+))?".
        Op().Regex(RegexOptions.IgnoreCase);
    #endregion
    #region 使用键值对集合
    /// <summary>
    /// 使用键值对集合初始化对象
    /// </summary>
    /// <param name="parameter">这个键值对集合枚举参数的名称和值</param>
    public UriParameter(IEnumerable<KeyValuePair<string, string?>> parameter)
    {
        Parameter = parameter.ToDictionary(true);
    }
    #endregion
    #region 使用元组集合
    /// <summary>
    /// 使用元组集合初始化对象
    /// </summary>
    /// <param name="parameter">这个元组集合枚举参数的名称和值</param>
    public UriParameter(IEnumerable<(string Name, string? Value)> parameter)
    {
        Parameter = parameter.ToDictionary(true);
    }
    #endregion
    #region 使用字符串
    /// <summary>
    /// 直接使用参数部分的字符串初始化对象
    /// </summary>
    /// <param name="uriParameter">Uri参数部分的字符串</param>
    public UriParameter(string uriParameter)
    {
        var (isMatch, matches) = Regex.Matches(uriParameter);
        if (!isMatch)
            throw new ArgumentException($"{uriParameter}不是合法的Uri参数字符串");
        Parameter = matches.Select(x =>
        {
            var key = x["key"].Match;
            var value = x.GroupsNamed.GetValueOrDefault("value")?.Match;
            return (key, value);
        }).ToDictionary(true);
    }
    #endregion
    #endregion
}
