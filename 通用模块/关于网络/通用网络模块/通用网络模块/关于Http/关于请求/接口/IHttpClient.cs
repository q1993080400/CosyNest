using System.IOFrancis.Bit;
using System.Net.Http.Json;
using System.Design.Direct;
using System.Text.Json;
using System.Design;

namespace System.NetFrancis.Http;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来发起Http请求
/// </summary>
public interface IHttpClient
{
    #region 说明文档
    /*问：BCL有原生的HttpClient，
      既然如此，为什么需要本类型？
      答：因为原生的HttpClient是可变的，
      而且经常在多个线程甚至整个应用中共享同一个HttpClient对象，
      这种操作非常危险，而本接口的所有API都是纯函数，消除了这个问题

      同时，本接口使用HttpRequestRecording来封装提交Http请求的信息，
      它是一个记录，通过C#9.0新支持的with表达式，能够更加方便的替换请求内容的任何部分
    
      问：本接口返回Json对象的请求都是直接返回IDirect，
      为什么将响应序列化为其他对象返回？
      答：因为这些API是为了请求不在你控制下的接口而设计的，
      往往需要二次封装响应结果，返回不需要事先声明的IDirect更加适合，
      如果你请求的接口就在你自己的服务器上，那么你应该使用强类型Http请求，
      它更加方便，而且可以返回具体的Json对象*/
    #endregion
    #region 直接返回HttpResponseMessage 
    /// <summary>
    /// 发起Http请求，并返回结果
    /// </summary>
    /// <param name="request">请求消息的内容</param>
    /// <param name="transformation">用来转换Http请求的函数，
    /// 如果为<see langword="null"/>，使用当前<see cref="IHttpClient"/>的默认配置，
    /// 注意：默认配置不等于不进行转换</param>
    /// <param name="cancellationToken">一个用于取消操作的令牌</param>
    /// <returns>Http请求的结果</returns>
    Task<HttpResponseMessage> Request(HttpRequestRecording request, HttpRequestTransform? transformation = null, CancellationToken cancellationToken = default);
    #endregion
    #region 返回Json对象
    /// <summary>
    /// 发起Http请求，并将结果反序列化为<see cref="IDirect"/>对象返回
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="RequestJsonPost{Obj}(string, Obj, ValueTuple{string, string}[], HttpRequestTransform?, JsonSerializerOptions?, CancellationToken)"/>
    async Task<IDirect> RequestJson(HttpRequestRecording request, HttpRequestTransform? transformation = null, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        using var response = await Request(request, transformation, cancellationToken);
        var newOptions = options.CanConverter(typeof(IDirect)) ?
           options : CreateDesign.JsonCommonOptions(JsonSerializerDefaults.Web);
        return await response.Content.ReadFromJsonAsync<IDirect>(newOptions, cancellationToken) ?? CreateDesign.DirectEmpty();
    }
    #endregion
    #region 发起Get请求，并返回Json对象
    /// <summary>
    /// 发起Get请求，并将结果反序列化为<see cref="IDirect"/>对象返回
    /// </summary>
    /// <param name="uri">请求的Uri</param>
    /// <param name="parameters">枚举请求的参数名称和值（如果有）</param>
    /// <param name="options">用来执行序列化的对象，
    /// 就算为<see langword="null"/>，它仍自带有对<see cref="IDirect"/>的支持</param>
    /// <inheritdoc cref="Request(HttpRequestRecording,HttpRequestTransform?,CancellationToken)"/>
    Task<IDirect> RequestJsonGet(string uri, (string Parameter, string? Value)[]? parameters = null, HttpRequestTransform? transformation = null,
        JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        => RequestJson(new HttpRequestRecording()
        {
            Uri = new(uri, parameters)
        }, transformation, options, cancellationToken);
    #endregion
    #region 发起Post请求，并返回Json对象
    /// <summary>
    /// 发起Post请求，并将结果反序列化为<see cref="IDirect"/>对象返回
    /// </summary>
    /// <param name="content">这个对象会被序列化，并放在请求体的内部</param>
    /// <typeparam name="Obj">请求参数的类型</typeparam>
    /// <inheritdoc cref="RequestJsonGet(string, ValueTuple{string, string}[], HttpRequestTransform?, JsonSerializerOptions?,CancellationToken)"/>
    async Task<IDirect> RequestJsonPost<Obj>(string uri, Obj content, (string Parameter, string? Value)[]? parameters = null,
       HttpRequestTransform? transformation = null, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        var requestRecording = new HttpRequestRecording()
        {
            Uri = new(uri, parameters),
            HttpMethod = HttpMethod.Post,
            Content = JsonContent.Create(content, null, options)
        };
        return await RequestJson(requestRecording, transformation, options, cancellationToken);
    }
    #endregion
    #region 返回IBitRead
    /// <summary>
    /// 发起Http请求，并以二进制管道的形式返回，
    /// 它可以用于下载
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Request(HttpRequestRecording, HttpRequestTransform?, CancellationToken)"/>
    /// <inheritdoc cref="RequestJsonGet(string, ValueTuple{string, string}[], HttpRequestTransform?, JsonSerializerOptions?,CancellationToken)"/>
    Task<IBitRead> RequestBitRead(string uri, CancellationToken cancellationToken = default);
    #endregion
    #region 强类型Http请求
    /// <summary>
    /// 返回一个对象，它可以用来发起强类型请求
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IHttpStrongTypeRequest{API}"/>
    IHttpStrongTypeRequest<API> StrongType<API>()
        where API : class;
    #endregion
}
