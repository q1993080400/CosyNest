using System.Text.RegularExpressions;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型表示一个完整的Uri
/// </summary>
public sealed record UriComplete : UriBase
{
    #region 隐式类型转换
    public static implicit operator UriComplete(string uri)
        => new(uri);
    #endregion
    #region 公开成员
    #region 主机部分
    /// <summary>
    /// 获取Uri的主机部分，
    /// 它包括协议，域名，端口等，
    /// 通过主机部分可以找到主机在网络上的位置
    /// </summary>
    public UriHost? UriHost { get; init; }
    #endregion
    #region 扩展部分
    /// <summary>
    /// 获取Uri的扩展部分，
    /// 它包括资源地址等，通过扩展部分，
    /// 可以知道应该访问主机的哪一个地方
    /// </summary>
    public UriExtend? UriExtend { get; init; }
    #endregion
    #region 参数部分
    private UriParameter? UriParameterField { get; set; }

    /// <summary>
    /// 获取Uri的参数部分
    /// </summary>
    public UriParameter? UriParameter
    {
        get => UriParameterField;
        init => UriParameterField = value is { Parameter.Count: > 0 } ? value : null;
    }
    #endregion
    #region 锚点部分
    /// <summary>
    /// 获取Uri的锚点部分
    /// </summary>
    public string? UriAnchor { get; init; }
    #endregion
    #region 是否为绝对地址
    /// <summary>
    /// 获取这个Uri是否为绝对地址
    /// </summary>
    public bool IsAbsolutely => UriHost is { };
    #endregion
    #region 完整Uri
    public override string Text
    {
        get
        {
            var uri = UriHost?.ToString();
            if (UriExtend is { })
                uri += UriExtend;
            if (UriParameter is { })
                uri += $"?{UriParameter}";
            if (UriAnchor is { })
                uri += $"#{UriAnchor}";
            return uri ?? "";
        }
    }
    #endregion
    #endregion
    #region 构造函数
    #region 指定Uri的文本形式
    #region 正则表达式
    /// <summary>
    /// 获取用于匹配Uri的正则表达式
    /// </summary>
    private static IRegex Regex { get; } =/*language=regex*/@"^(?<host>([a-z0-9]+://)?[^/]+)?(/?(?<extend>[^#\?]+)(\?(?<parameter>[^#]+))?)?(#(?<anchor>\S+))?$".
        Op().Regex(RegexOptions.IgnoreCase);
    #endregion
    #region 指定Uri和参数
    /// <summary>
    /// 使用指定的Uri初始化对象
    /// </summary>
    /// <param name="uri">完整的Uri</param>
    /// <param name="parameters">枚举Uri参数的名称和值，
    /// 如果<paramref name="uri"/>里面已经有了参数，然后又指定了这个参数，则会引发异常</param>
    public UriComplete(string uri, (string Parameter, string? Value)[]? parameters = null)
    {
        var match = Regex.MatcheSingle(uri) ?? throw new ArgumentException($"{uri}不是合法的Uri");
        if (match.GroupsNamed.TryGetValue("host", out var host))
            UriHost = new(host.Match);
        if (match.GroupsNamed.TryGetValue("extend", out var extend))
            UriExtend = new(extend.Match);
        if ((UriHost, UriExtend) is (null, null))
            throw new ArgumentException($"{uri}不是合法的Uri，它既没有主机部分，也没有扩展部分");
        if (match.GroupsNamed.TryGetValue("parameter", out var parameter))
        {
            if (parameters is { Length: > 0 })
                throw new NotSupportedException($"在{nameof(uri)}已经包含参数的情况下，不能再次显式指定参数");
            UriParameter = new(parameter.Match);
        }
        else
        {
            UriParameter = parameters is null ? null : new(parameters);
        }
        if (match.GroupsNamed.TryGetValue("anchor", out var anchor))
            UriAnchor = anchor.Match;
    }
    #endregion
    #endregion 
    #region 无参数构造函数
    /// <summary>
    /// 无参数构造函数
    /// </summary>
    public UriComplete()
    {

    }
    #endregion
    #endregion
}
