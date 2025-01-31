using System.Net.Http.Headers;

namespace System.NetFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Http标头
/// </summary>
public interface IHttpHeader
{
    #region 索引所有标头属性
    /// <summary>
    /// 获取一个索引所有标头属性的字典，
    /// 它的键是属性的名称，值是属性的值
    /// </summary>
    /// <returns></returns>
    IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }
    #endregion
    #region 将标头复制到HttpHeaders
    /// <summary>
    /// 将本对象的所有标头复制到另一个<see cref="HttpHeaders"/>中，
    /// 然后返回这个<see cref="HttpHeaders"/>
    /// </summary>
    /// <typeparam name="Headers">Http标头的类型</typeparam>
    /// <param name="header">本对象的所有标头将被复制到这个参数中</param>
    Headers CopyHeader<Headers>(Headers header)
        where Headers : HttpHeaders;
    #endregion
}
