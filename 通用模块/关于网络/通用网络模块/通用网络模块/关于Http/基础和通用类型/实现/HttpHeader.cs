using System.Collections.Immutable;
using System.Net.Http.Headers;

namespace System.NetFrancis.Http.Realize;

/// <summary>
/// 这个记录是Http标头的可选基类
/// </summary>
public abstract record HttpHeader : IHttpHeader
{
    #region 公开成员
    #region 标头属性
    public IReadOnlyDictionary<string, IEnumerable<string>> Headers => HeadersImmutable;
    #endregion
    #region 将标头复制到HttpHeaders
    public Headers CopyHeader<Headers>(Headers header)
          where Headers : HttpHeaders
    {
        header.Clear();
        foreach (var (key, value) in this.Headers)
        {
            header.Add(key, value);
        }
        return header;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 标头字典
    /// <summary>
    /// 获取标头字典
    /// </summary>
    protected ImmutableDictionary<string, IEnumerable<string>> HeadersImmutable { get; set; }
        = ImmutableDictionary<string, IEnumerable<string>>.Empty;
    #endregion
    #region 获取标头
    /// <summary>
    /// 获取标头的模板方法，如果键存在，
    /// 则通过一个委托获取值，否则返回<see langword="null"/>
    /// </summary>
    /// <typeparam name="Header">标头类型</typeparam>
    /// <param name="key">用来获取标头的键</param>
    /// <param name="ifExist">如果标头的键存在，根据标头的值返回标头的委托</param>
    /// <returns></returns>
    protected Header? GetHeader<Header>(string key, Func<IEnumerable<string>, Header> ifExist)
        where Header : class
    {
        var value = HeadersImmutable.TryGetValue(key).Value?.ToArray();
        return value is { Length: > 0 } ? ifExist(value) : null;
    }
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
        HeadersImmutable = value is null ?
            HeadersImmutable.Remove(key) :
            HeadersImmutable.SetItem(key, convertValue(value));
    }
    #endregion
    #endregion
    #region 抽象成员
    #region 改变标头
    /// <summary>
    /// 改变标头属性，并返回一个新的标头
    /// </summary>
    /// <param name="change">这个委托传入旧的标头，返回新的标头</param>
    /// <returns></returns>
    public abstract HttpHeader With
        (Func<ImmutableDictionary<string, IEnumerable<string>>, ImmutableDictionary<string, IEnumerable<string>>> change);
    #endregion
    #endregion
    #region 重写的ToString方法
    public sealed override string ToString()
        => HeadersImmutable.Join(static x => $"{x.Key}:{x.Value.Join(",")}", Environment.NewLine);
    #endregion
    #region 构造函数
    #region 传入标头集合
    /// <summary>
    /// 使用指定的标头集合初始化对象，
    /// 这个构造函数可以使<see cref="HttpHeaders"/>及其派生类能够更方便的转换为<see cref="IHttpHeader"/>
    /// </summary>
    /// <param name="headers">使用指定的自定义标头集合初始化对象</param>
    public HttpHeader(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
    {
        var h = headers.Select(static x => new KeyValuePair<string, IEnumerable<string>>
        (x.Key, x.Value.Where(static x => !x.IsVoid()).ToArray())).
            Where(static x => (x.Key.IsVoid(), x.Value.Count()) is (false, > 0)).ToArray();
        HeadersImmutable = ImmutableDictionary<string, IEnumerable<string>>.Empty.AddRange(h);
    }
    #endregion
    #endregion
}
