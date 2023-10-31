using System.Net.Http.Headers;

namespace System.NetFrancis.Http.Realize;

/// <summary>
/// 这个记录是Http标头的可选基类
/// </summary>
public abstract record HttpHeader : IHttpHeader
{
    #region 公开成员
    #region 标头属性
    #region 不可变字典
    IReadOnlyDictionary<string, IEnumerable<string>> IHttpHeader.Headers => Headers;
    #endregion
    #region 可变字典
    /// <summary>
    /// 获取标头字典，它是可变的
    /// </summary>
    public Dictionary<string, IEnumerable<string>> Headers { get; } = [];
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 获取标头
    /// <summary>
    /// 获取标头的模板方法，如果键存在，
    /// 则通过一个委托获取值，否则返回<see langword="null"/>
    /// </summary>
    /// <typeparam name="Header">标头类型</typeparam>
    /// <param name="key">用来获取标头的键</param>
    /// <param name="ifExist"></param>
    /// <returns></returns>
    protected Header? GetHeader<Header>(string key, Func<IEnumerable<string>, Header> ifExist)
        where Header : class
        => Headers.TryGetValue(key).Value is { } v ?
        ifExist(v) : null;
    #endregion
    #region 设置标头
    /// <summary>
    /// 设置标头的模板方法，如果要写入的值为<see langword="null"/>，
    /// 则移除指定的标头，否则将标头转换后写入
    /// </summary>
    /// <param name="value">要写入的标头</param>
    /// <param name="convertValue">用来转换标头写入值的函数</param>
    /// <inheritdoc cref="GetHeader{Header}(string, Func{IEnumerable{string}, Header})"/>
    protected void SetHeader<Header>(string key, Header? value, Func<Header, IEnumerable<string>> convertValue)
    {
        if (value is null)
            Headers.Remove(key);
        else
            Headers[key] = convertValue(value);
    }
    #endregion
    #endregion
    #region 重写的ToString方法
    public sealed override string ToString()
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
            Where(x => (x.Key.IsVoid(), x.Item2.Length) is (false, > 0)).ToArray();
        foreach (var (k, v) in h)
        {
            if (Headers.TryGetValue(k, out var value))
            {
                Headers[k] = value.Concat(v).ToArray();
            }
            else
                Headers[k] = v;
        }
    }
    #endregion
    #endregion
}
