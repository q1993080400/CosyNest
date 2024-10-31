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
    #region 说明文档
    /*问：应该如何表示需要上传功能的强类型Http请求？
      答：按照规范，如果某个API需要使用上传功能，
      那么要上传的二进制流应该在方法参数中应该为UploadFile或IEnumerable<UploadFile>，
      程序会正确地识别出它的作用，并把它转换为对应的Http上下文
    
      问：但是，后端的控制器并不认识UploadFile，在AspNetCore中，
      接受流的控制器参数类型应该为IFormFile，而这个类型在前端不存在，
      应该如何处理这个矛盾？
      答：在中台接口中，仍然使用UploadFile表示上传流，然后让控制器实现这个接口，
      但是用NonActionAttribute将这个方法从路由中排除，
      然后在控制器中声明一个新的方法，除了将UploadFile的参数类型替换为IFormFile以外，
      它的所有签名都跟中台接口中的那个方法一模一样，它是实际被执行的控制器方法
     
      问：IFormFile参数是通过Http表单获取的，如果我在上传的过程中，
      需要传入其他的参数，那么应该怎么办？
      答：你可以把其他参数用FromHeaderAttribute特性修饰，表示它应该从Http标头中获取，
      有一个专用的自定义标头叫ObjectHeaderValue，它是专门用来处理这个问题的*/
    #endregion
    #region 强类型Http请求
    #region 返回HttpResponseMessage 
    /// <summary>
    /// 发起强类型Http请求，并返回结果
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="request">用于描述请求路径和参数的表达式</param>
    /// <param name="transformation">用来转换请求的委托</param>
    /// <param name="options">一个用于Json转换的对象，它仅用来序列化请求，不用来反序列化响应</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task<HttpResponseMessage> RequestResponse<Ret>(Expression<Func<API, Ret>> request,
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
    #region 不返回任何对象
    /// <summary>
    /// 发起强类型Http请求，并将结果反序列化为Json后返回
    /// </summary>
    /// <inheritdoc cref="RequestResponse{Ret}(Expression{Func{API, Ret}}, HttpRequestTransform?, JsonSerializerOptions?, CancellationToken)"/>
    async Task Request(Expression<Func<API, Task>> request, HttpRequestTransform? transformation = null,
       JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        using var response = await RequestResponse(request, transformation, options, cancellationToken);
    }
    #endregion
    #region 返回Json对象
    /// <summary>
    /// 发起强类型Http请求，并将结果反序列化为Json后返回
    /// </summary>
    /// <param name="options">一个用于Json转换的对象，它用来转换请求和响应</param>
    /// <inheritdoc cref="RequestResponse{Ret}(Expression{Func{API, Ret}}, HttpRequestTransform?, JsonSerializerOptions?, CancellationToken)"/>
    async Task<Ret> Request<Ret>(Expression<Func<API, Task<Ret>>> request, HttpRequestTransform? transformation = null,
       JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        using var response = await RequestResponse(request, transformation, options, cancellationToken);
        return await response.Content.ReadFromJsonOrStringAsync<Ret>(options, cancellationToken);
    }
    #endregion
    #endregion
}
