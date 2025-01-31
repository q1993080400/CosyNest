using System.DataFrancis;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace System.NetFrancis;

/// <summary>
/// 这个类型是<see cref="IHttpStrongTypeInvoke{API}"/>的实现，
/// 它通过发起Http请求来实现远程调用
/// </summary>
/// <param name="httpClient">封装的<see cref="IHttpClient"/>对象，
/// 本对象通过它发起请求</param>
/// <inheritdoc cref="IHttpStrongTypeInvoke{API}"/>
sealed class HttpStrongTypeInvoke<API>(IHttpClient httpClient) : IHttpStrongTypeInvoke<API>
    where API : class
{
    #region 接口实现
    #region 直接返回Http响应
    public async Task<HttpResponseMessage> RequestResponse<Ret>(Expression<Func<API, Ret>> invoke, CancellationToken cancellationToken)
    {
        if (invoke is not
            {
                Body: MethodCallExpression
                {
                    Object: ParameterExpression,
                    Arguments: { } arguments,
                    Method: { } method,
                }
            })
            throw new NotSupportedException($"表达式中只允许包含直接对{typeof(API)}方法的调用");
        var requestPack = GetRequest(method, arguments);
        return await httpClient.Request(requestPack, cancellationToken: cancellationToken);
    }
    #endregion
    #region 无返回值
    public async Task Invoke(Expression<Func<API, Task>> invoke, CancellationToken cancellationToken = default)
    {
        using var response = await RequestResponse(invoke, cancellationToken);
    }
    #endregion
    #region 有返回值
    public async Task<Ret> Invoke<Ret>(Expression<Func<API, Task<Ret>>> invoke, CancellationToken cancellationToken = default)
    {
        using var response = await RequestResponse(invoke, cancellationToken);
        return await response.Content.ReadFromJsonOrStringAsync<Ret>(cancellationToken: cancellationToken);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 生成HTTP请求
    /// <summary>
    /// 辅助方法：根据表达式，
    /// 生成HTTP请求对象
    /// </summary>
    /// <param name="method">调用的API方法，
    /// 他会被映射到一个控制器操作</param>
    /// <param name="arguments">调用API传入的方法参数</param>
    /// <returns></returns>
    private static HttpRequestRecording GetRequest(MethodInfo method, IEnumerable<Expression> arguments)
    {
        var callPath = GetCallUri(method);
        var prameterInfos = method.GetParameters().
            Zip(arguments).
            Select(static x => new HttpStrongTypeInvokeParameterInfo(x.First, x.Second.CalValue())).
            ToArray();
        var httpMethod = GetRequestMethod(method, prameterInfos);
        var uriParameter = GetRequestUriParameter(method, httpMethod, prameterInfos);
        var httpContent = GetRequestContent(method, httpMethod, prameterInfos);
        var httpRequest = new HttpRequestRecording()
        {
            Uri = callPath with
            {
                UriParameter = uriParameter
            },
            HttpMethod = httpMethod,
            Content = httpContent,
        };
        return httpRequest;
    }
    #endregion
    #region 生成调用Uri
    /// <summary>
    /// 生成对WebApi的调用Uri
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetRequest"/>
    private static UriComplete GetCallUri(MethodInfo method)
    {
        if (method.GetCustomAttribute<RouteDescriptionAttribute>() is { Template: { } methodTemplate })
            return methodTemplate;
        var interfaceType = typeof(API);
        var interfaceName = interfaceType.Name.TrimStart('I').Trim(false, "Controller");
        var methodName = method.Name;
        if (interfaceType.GetCustomAttribute<RouteDescriptionAttribute>() is { Template: { } interfaceTemplate })
            return interfaceTemplate.Replace("[controller]", interfaceName).
                Replace("[action]", methodName);
        return $"/api/{interfaceName}/{methodName}";        //如果没有任何特性，则使用默认的调用路径
    }
    #endregion
    #region 判断应该使用哪个Http方法
    /// <summary>
    /// 判断该强类型Http请求应该使用哪个Http方法
    /// </summary>
    /// <param name="method">强类型Http请求的方法对象</param>
    /// <param name="prameterInfos">描述请求参数的对象</param>
    /// <returns></returns>
    private static HttpMethod GetRequestMethod(MethodInfo method, IEnumerable<HttpStrongTypeInvokeParameterInfo> prameterInfos)
    {
        var appoint = method.GetCustomAttribute<HttpRequestMethodAttribute>()?.
            Method ?? HttpInvokeMethod.Auto;
        switch (appoint)
        {
            case HttpInvokeMethod.Auto:
                var isPost = prameterInfos.Any(static x =>
                x is { ParameterSource: HttpInvokeParameterSource.FromBody } or
                { IsCommonType: false });
                return isPost ? HttpMethod.Post : HttpMethod.Get;
            case HttpInvokeMethod.Get:
                return HttpMethod.Get;
            case HttpInvokeMethod.Post:
                return HttpMethod.Post;
            default:
                throw appoint.Unrecognized();
        }
    }
    #endregion
    #region 生成Uri参数
    /// <summary>
    /// 生成请求中Uri的参数部分
    /// </summary>
    /// <param name="methodInfo">强类型Http请求的方法</param>
    /// <param name="httpMethod">请求所使用的Http方法</param>
    /// <param name="prameterInfos">描述请求参数的对象</param>
    /// <returns></returns>
    private static UriParameter GetRequestUriParameter(MethodInfo methodInfo, HttpMethod httpMethod, IEnumerable<HttpStrongTypeInvokeParameterInfo> prameterInfos)
    {
        #region 用来枚举参数名称和值的本地函数
        IEnumerable<(string Name, string? Value)> AllUriParameter()
        {
            foreach (var prameterInfo in prameterInfos)
            {
                switch (prameterInfo)
                {
                    case { ParameterSource: HttpInvokeParameterSource.FromQuery }:
                    case { ParameterSource: HttpInvokeParameterSource.Auto } when httpMethod == HttpMethod.Get:
                        var name = prameterInfo.Parameter.Name!;
                        if (!prameterInfo.IsCommonType)
                            throw new NotSupportedException($"{typeof(API)}.{methodInfo.Name}的{name}参数是一个复杂的类型，" +
                            $"它不能放在HttpGet方法中，或被指定为来自查询Uri参数");
                        var value = prameterInfo.Value;
                        if (value is { })
                            yield return (name, value.ToString());
                        break;
                }
            }
        }
        #endregion
        var parameter = AllUriParameter().ToArray();
        return new(parameter);
    }
    #endregion
    #region 生成请求内容
    /// <summary>
    /// 生成请求内容，并返回
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetRequestUriParameter(MethodInfo, HttpMethod, IEnumerable{HttpStrongTypeInvokeParameterInfo})"/>
    private static HttpContent? GetRequestContent(MethodInfo methodInfo, HttpMethod httpMethod, IEnumerable<HttpStrongTypeInvokeParameterInfo> prameterInfos)
    {
        if (httpMethod == HttpMethod.Get)
            return null;
        var filterParameter = prameterInfos.
            Where(static x => x is { IsCommonType: false } or { ParameterSource: HttpInvokeParameterSource.FromBody }).ToArray();
        return filterParameter switch
        {
            null or [] => null,
            [var info] => GetRequestContentFinal(info),
            _ => throw new NotSupportedException($"{typeof(API)}.{methodInfo.Name}方法存在多个应该被封装进请求体的参数，无法生成请求内容"),
        };
    }
    #endregion
    #region 生成最终请求内容
    /// <summary>
    /// 生成最终的请求内容
    /// </summary>
    /// <param name="info">请求参数的信息</param>
    private static HttpContent GetRequestContentFinal(HttpStrongTypeInvokeParameterInfo info)
    {
        var value = info.Value;
        var valueType = info.Parameter.ParameterType;
        var previewFileTypeInfo = CreateDataObj.GetPreviewFileTypeInfo(valueType);
        return previewFileTypeInfo.HasPreviewFile ?
            HasUpload() : NotUpload();
        #region 包含文件上传的请求
        MultipartContent HasUpload()
        {
            var multipartContent = new MultipartFormDataContent();
            var writeFile = CreateNet.UploadFileResolverModifiersWrite((file, id) =>
            {
                var fileContent = new StreamContent(file.OpenFileStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                multipartContent.Add(fileContent, $"\"{id}\"", file.FileName);
            });
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                {
                    Modifiers = { writeFile }
                }
            };
            var jsonContent = JsonContent.Create(value, valueType, options: options);
            multipartContent.Add(jsonContent, IHasPreviewFile.ContentKey);
            return multipartContent;
        }
        #endregion
        #region 不包含文件上传的请求
        JsonContent NotUpload()
        {
            var options = JsonSerializerOptions.Web;
            return JsonContent.Create(value, valueType, options: options);
        }
        #endregion
    }
    #endregion
    #endregion
}
