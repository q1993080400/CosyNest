using System.Net.Http.Headers;

namespace System.NetFrancis.Http.Realize;

/// <summary>
/// 这个记录是Http标头的可选基类
/// </summary>
public abstract record HttpHeader : IHttpHeader
{
    #region 公开成员
    #region 标头属性
    #region 正式属性
    public IReadOnlyDictionary<string, IEnumerable<string>> Headers => HeadersVar;
    #endregion
    #region 标头字典
    /// <summary>
    /// 获取标头字典，它是可变的
    /// </summary>
    protected Dictionary<string, IEnumerable<string>> HeadersVar { get; } = new();
    #endregion
    #endregion
    #endregion
    #region 重写的ToString方法
    public override string ToString()
        => Headers.Join(x => $"{x.Key}:{x.Value.Join(",")}", Environment.NewLine);
    #endregion
    #region 构造函数
    #region 无参数构造函数
    public HttpHeader()
    {

    }
    #endregion
    #region 传入标头集合
    /// <summary>
    /// 使用指定的标头集合初始化对象，
    /// 这个构造函数可以使<see cref="HttpHeaders"/>及其派生类能够更方便的转换为<see cref="IHttpHeader"/>
    /// </summary>
    /// <param name="headers">使用指定的自定义标头集合初始化对象</param>
    public HttpHeader(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
    {
        var h = headers.Select(x => (x.Key, x.Value.Where(x => !x.IsVoid()).ToArray())).
            Where(x => !x.Key.IsVoid() && x.Item2.Any()).ToArray();
        foreach (var (k, v) in h)
        {
            if (HeadersVar.TryGetValue(k, out var value))
            {
                HeadersVar[k] = value.Concat(v).ToArray();
            }
            else
                HeadersVar[k] = v;
        }
    }
    #endregion
    #endregion
}
