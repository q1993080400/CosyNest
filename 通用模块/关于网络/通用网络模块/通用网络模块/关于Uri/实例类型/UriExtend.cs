using System.Text.RegularExpressions;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型代表Uri的扩展部分，
/// 它包括资源地址等
/// </summary>
public sealed record UriExtend : UriBase
{
    #region 隐式类型转换
    public static implicit operator UriExtend(string uri)
        => new(uri);
    #endregion
    #region 公开成员
    #region 扩展地址
    public override string Text { get; }
    #endregion
    #endregion
    #region 构造函数
    #region 正则表达式
    /// <summary>
    /// 获取用来匹配扩展地址的正则表达式
    /// </summary>
    private static IRegex Regex { get; } =/*language=regex*/@"^(\w|\.|-|/)+$".
        Op().Regex(RegexOptions.IgnoreCase);
    #endregion
    #region 正式方法
    /// <summary>
    /// 使用扩展地址初始化对象
    /// </summary>
    /// <param name="extend">扩展地址，它包括资源地址等</param>
    public UriExtend(string extend)
    {
        var match = Regex.MatcheSingle(extend) ?? throw new ArgumentException($"{extend}不是合法的扩展地址");
        Text = "/" + match.Match.TrimEnd('/').TrimStart('/');
    }
    #endregion
    #endregion
}
