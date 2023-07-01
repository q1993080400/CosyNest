using System.Text.RegularExpressions;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型代表Uri的主机部分，
/// 它包括协议，域名，端口号等等
/// </summary>
public sealed record UriHost : UriBase
{
    #region 隐式类型转换
    public static implicit operator UriHost(string uri)
        => new(uri);
    #endregion
    #region 公开成员
    #region 协议
    /// <summary>
    /// 获取主机的协议
    /// </summary>
    public string Agreement { get; init; }
    #endregion
    #region 域名
    /// <summary>
    /// 获取主机的域名
    /// </summary>
    public string DomainName { get; init; }
    #endregion
    #region 端口
    /// <summary>
    /// 获取主机的端口
    /// </summary>
    public int Port
        => PortExplicit switch
        {
            { } p => p,
            null => Agreement switch
            {
                "http" => 80,
                "https" => 443,
                var agreement => throw new NotSupportedException($"协议{agreement}是未知的，不知道它的默认端口")
            }
        };
    #endregion
    #region 主机地址
    public override string Text
    {
        get
        {
            var text = $"{Agreement}://{DomainName}";
            if (PortExplicit is { })
                text += $":{PortExplicit}";
            return text;
        }
    }
    #endregion
    #endregion
    #region 内部成员
    #region 显式端口
    /// <summary>
    /// 获取主机显式指定的端口，
    /// 它不包括默认端口
    /// </summary>
    private int? PortExplicit { get; init; }
    #endregion
    #endregion
    #region 构造函数
    #region 正则表达式
    /// <summary>
    /// 获取用来匹配主机地址的正则表达式
    /// </summary>
    private static IRegex Regex { get; } =/*language=regex*/@"^(?<agreement>[a-z]+)://(?<domainName>(\w|\.|-)+)(:(?<port>\d+))?/?$".
        Op().Regex(RegexOptions.IgnoreCase);
    #endregion
    #region 正式方法
    /// <summary>
    /// 使用主机地址初始化对象
    /// </summary>
    /// <param name="host">主机的地址，
    /// 它包括协议，域名，端口号等</param>
    public UriHost(string host)
    {
        var match = Regex.MatcheSingle(host) ?? throw new ArgumentException($"{host}不是合法的主机字符串");
        Agreement = match["agreement"].Match;
        DomainName = match["domainName"].Match;
        if (match.GroupsNamed.TryGetValue("port", out var port))
            PortExplicit = port.Match.To<int>();
    }
    #endregion
    #endregion
}
