using System.IOFrancis.Bit;
using System.Net.Http.Json;
using System.Design.Direct;
using System.Linq.Expressions;
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

      同时，本接口使用IHttpRequestRecording来封装提交Http请求的信息，
      根据推荐做法，通常使用该接口的实现HttpRequestRecording，
      它是一个记录，通过C#9.0新支持的with表达式，能够更加方便的替换请求内容的任何部分
    
      问：在本接口的早期版本，曾提供了一些API，
      发起请求并将结果转换为文本，Json等对象直接返回，
      这些方法非常方便，但是为什么后来被删除了？
      答：出于以下原因：

      #根据对象职责明确原则，本类型只负责发起请求，而不负责解释请求的结果，
      这个职责由IHttpContent以及它的扩展方法负责执行，如果由本类型负责这个任务的话，
      会导致接口非常臃肿，例如，必须为Get和Post方法分别声明返回文本，Json的方法
    
      #本类型发起Http请求的方法返回IHttpResponse，
      它携带了有关错误码等等的信息，很多时候需要这些信息*/
    #endregion
    #region 发起Http请求（指定IHttpRequestRecording）
    #region 直接返回IHttpResponse
    /// <summary>
    /// 发起Http请求，并返回结果
    /// </summary>
    /// <param name="request">请求消息的内容</param>
    /// <param name="transformation">用来转换Http请求的函数，
    /// 如果为<see langword="null"/>，使用当前<see cref="IHttpClient"/>的默认配置，
    /// 注意：默认配置不等于不进行转换</param>
    /// <param name="cancellationToken">一个用于取消操作的令牌</param>
    /// <returns>Http请求的结果</returns>
    Task<IHttpResponse> Request(HttpRequestRecording request, HttpRequestTransform? transformation = null, CancellationToken cancellationToken = default);
    #endregion
    #region 返回IBitRead
    /// <summary>
    /// 发起Http请求，并以二进制管道的形式返回，
    /// 它可以用于下载
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Request(HttpRequestRecording, HttpRequestTransform?, CancellationToken)"/>
    Task<IBitRead> RequestDownload(HttpRequestRecording request, HttpRequestTransform? transformation = null, CancellationToken cancellationToken = default);
    #endregion
    #endregion
    #region 发起Http请求（指定Uri）
    #region 直接返回IHttpResponse
    /// <param name="uri">请求的Uri</param>
    /// <param name="parameters">枚举请求的参数名称和值（如果有）</param>
    /// <inheritdoc cref="Request(HttpRequestRecording,HttpRequestTransform?,CancellationToken)"/>
    Task<IHttpResponse> Request(string uri, (string Parameter, string? Value)[]? parameters = null, HttpRequestTransform? transformation = null, CancellationToken cancellationToken = default)
        => Request(new HttpRequestRecording(uri, parameters), transformation, cancellationToken);
    #endregion
    #region 返回IBitRead
    /// <inheritdoc cref="RequestDownload(HttpRequestRecording,HttpRequestTransform?,CancellationToken)"/>
    /// <inheritdoc cref="Request(string, ValueTuple{string,string}[],HttpRequestTransform?,CancellationToken)"/>
    Task<IBitRead> RequestDownload(string uri, (string Parameter, string? Value)[]? parameters = null, HttpRequestTransform? transformation = null, CancellationToken cancellationToken = default)
        => RequestDownload(new HttpRequestRecording(uri, parameters), transformation, cancellationToken);
    #endregion
    #region Post请求
    /// <summary>
    /// 发起Post请求，并返回结果
    /// </summary>
    /// <param name="content">这个对象会被序列化，并放在请求体的内部</param>
    /// <param name="options">用来序列化<paramref name="content"/>的对象，
    /// 就算为<see langword="null"/>，它仍自带有对<see cref="IDirect"/>的支持</param>
    /// <param name="parameters">请求的Uri参数名称和值，
    /// 是的，即便是Post请求，它也可以包含Uri参数</param>
    /// <inheritdoc cref="Request(string, ValueTuple{string,string}[],HttpRequestTransform?, CancellationToken)"/>
    async Task<IHttpResponse> RequestPost(string uri, object content, JsonSerializerOptions? options = null,
        (string Parameter, string? Value)[]? parameters = null, HttpRequestTransform? transformation = null, CancellationToken cancellationToken = default)
    {
        var json = JsonContent.Create(content, options: options ?? CreateDesign.JsonCommonOptions);
#if DEBUG
        var jsonText = await json.ReadAsStringAsync(cancellationToken);
#endif
        var request = new HttpRequestRecording(uri, parameters) with
        {
            HttpMethod = HttpMethod.Post,
            Content = await json.ToHttpContent()
        };
        return await Request(request, transformation, cancellationToken);
    }
    #endregion
    #endregion
    #region 强类型Http请求
    #region 返回IHttpResponse
    /// <summary>
    /// 发起强类型Http请求，并返回结果
    /// </summary>
    /// <typeparam name="API">API接口的类型</typeparam>
    /// <param name="request">用于描述请求路径和参数的表达式</param>
    /// <param name="options">一个用于执行Json转换的对象</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    /// <inheritdoc cref="Request(HttpRequestRecording, HttpRequestTransform?, CancellationToken)"/>
    Task<IHttpResponse> Request<API>(Expression<Func<API, object?>> request, HttpRequestTransform? transformation = null,
        JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        where API : class;

    /*问：如何使用这个方法？
      答：泛型参数API一般是一个接口，放在中台，并被控制器实现，
      然后前端在这个方法中传入这个泛型参数，
      并在request参数中调用这个接口的方法，
      函数会将表达式翻译为对WebApi的调用
    
      这个接口方法的返回值可以为任意类型，
      因为该接口方法实际上不会被执行*/
    #endregion
    #region 返回Json对象
    /// <summary>
    /// 发起强类型Http请求，并将结果反序列化为Json后返回，
    /// 它只支持Get请求
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <returns></returns>
    /// <inheritdoc cref="Request{API}(Expression{Func{API, object?}},HttpRequestTransform?,JsonSerializerOptions?, CancellationToken)"/>
    async Task<Ret> Request<API, Ret>(Expression<Func<API, Task<Ret>>> request, HttpRequestTransform? transformation = null,
        JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        where API : class
    {
        var expression = Expression.Lambda<Func<API, object?>>(request.Body, request.Parameters);
        return await Request(expression, transformation, options, cancellationToken).Read(x => x.ToObject<Ret>(options));
    }
    #endregion
    #endregion
}
