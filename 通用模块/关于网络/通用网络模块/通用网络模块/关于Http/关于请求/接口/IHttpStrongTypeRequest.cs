using System.Linq.Expressions;
using System.Text.Json;

namespace System.NetFrancis.Http;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Http强类型请求
/// </summary>
/// <typeparam name="API">强类型API的类型</typeparam>
public interface IHttpStrongTypeRequest<API>
    where API : class
{
    #region 强类型Http请求
    #region 返回IHttpResponse
    /// <summary>
    /// 发起强类型Http请求，并返回结果
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="request">用于描述请求路径和参数的表达式</param>
    /// <param name="transformation">用来转换请求的委托</param>
    /// <param name="options">一个用于Json转换的对象</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task<IHttpResponse> RequestResponse<Ret>(Expression<Func<API, Ret>> request,
        HttpRequestTransform? transformation = null, JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default);

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
    /// 发起强类型Http请求，并将结果反序列化为Json后返回
    /// </summary>
    /// <inheritdoc cref="RequestResponse{Ret}(Expression{Func{API, Ret}}, HttpRequestTransform?, JsonSerializerOptions?, CancellationToken)"/>
    Task<Ret> Request<Ret>(Expression<Func<API, Task<Ret>>> request, HttpRequestTransform? transformation = null,
       JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
       => RequestResponse(request, transformation, options, cancellationToken).
           Read(x => x.ToObject<Ret>(options));
    #endregion
    #endregion
}
