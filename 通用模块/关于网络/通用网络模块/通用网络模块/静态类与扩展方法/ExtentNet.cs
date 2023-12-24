using System.Net;
using System.Net.Http.Headers;
using System.NetFrancis;
using System.NetFrancis.Http;
using System.Text;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace System;

/// <summary>
/// 有关网络的扩展方法全部放在这里
/// </summary>
public static class ExtendNet
{
    #region 有关IHttpClient及其衍生类型
    #region 公开成员
    #region 将HttpClient转换为IHttpClient
    /// <summary>
    /// 返回一个<see cref="HttpClient"/>的<see cref="IHttpClient"/>包装
    /// </summary>
    /// <param name="httpClient">待包装的<see cref="HttpClient"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="HttpClientRealize(HttpClient, HttpRequestTransform?)"/>
    public static IHttpClient ToHttpClient(this HttpClient httpClient, HttpRequestTransform? requestTransform = null)
        => new HttpClientRealize(httpClient, requestTransform);
    #endregion
    #region 读取IHttpResponse的内容，并将其转换为更高层的类型
    /// <summary>
    /// 读取一个<see cref="IHttpResponse"/>的内容，
    /// 并将其转换为更高层次的类型返回
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="response">待读取的<see cref="IHttpResponse"/></param>
    /// <param name="read">这个委托传入待读取的<see cref="IHttpContent"/>，返回转换后的类型</param>
    /// <returns></returns>
    public static async Task<Ret> Read<Ret>(this Task<IHttpResponse> response, Func<IHttpContent, Task<Ret>> read)
    {
        var r = await response;
        return await read(r.Content);
    }
    #endregion
    #region HttpContent转换为IHttpContent
    /// <summary>
    /// 将<see cref="HttpContent"/>转换为等效的<see cref="IHttpContent"/>
    /// </summary>
    /// <param name="content">待转换的<see cref="HttpContent"/></param>
    /// <returns></returns>
    public static async Task<HttpContentRecording?> ToHttpContent(this HttpContent? content)
    {
        if (content is null)
            return null;
        var array = await content.ReadAsByteArrayAsync();
        return new HttpContentRecording()
        {
            Content = array.ToBitRead(),
            Header = new(content.Headers)
        };
    }
    #endregion
    #endregion
    #region 内部成员
    #region 将HttpRequestRecording转换为HttpRequestMessage
    /// <summary>
    /// 将<see cref="HttpRequestRecording"/>转换为等效的<see cref="HttpRequestMessage"/>
    /// </summary>
    /// <param name="recording">待转换的<see cref="HttpRequestRecording"/></param>
    /// <param name="baseAddress">请求目标Uri的基地址</param>
    /// <returns></returns>
    internal static async Task<HttpRequestMessage> ToHttpRequestMessage(this HttpRequestRecording recording, Uri? baseAddress)
    {
        var uri = recording.Uri;
        var m = new HttpRequestMessage()
        {
            RequestUri = baseAddress is null ? new(uri) : new(baseAddress, uri),
            Method = recording.HttpMethod,
            Content = await recording.Content.ToHttpContent(),
        };
        recording.Header.CopyHeader(m.Headers);
        return m;
    }
    #endregion
    #region 将IHttpContent转换为HttpContent
    /// <summary>
    /// 将<see cref="IHttpContent"/>转换为<see cref="HttpContent"/>
    /// </summary>
    /// <param name="content">待转换的<see cref="HttpContent"/></param>
    /// <returns></returns>
    private static async Task<HttpContent?> ToHttpContent(this IHttpContent? content)
    {
        if (content is null)
            return null;
        var array = await content.Content.ReadComplete();
        var arrayContent = new ByteArrayContent(array);
        content.Header.CopyHeader(arrayContent.Headers);
        return arrayContent;
    }
    #endregion
    #region 将IHttpHeader的标头复制到HttpHeaders
    /// <summary>
    /// 将<see cref="IHttpHeader"/>的所有标头复制到另一个<see cref="HttpHeaders"/>中
    /// </summary>
    /// <param name="header">待复制标头的<see cref="IHttpHeader"/></param>
    /// <param name="bclHeader"><paramref name="header"/>的所有标头将被复制到这个参数中</param>
    internal static void CopyHeader(this IHttpHeader header, HttpHeaders bclHeader)
    {
        bclHeader.Clear();
        foreach (var (key, value) in header.Headers)
        {
            bclHeader.Add(key, value);
        }
    }
    #endregion
    #region 将HttpResponseMessage转换为HttpResponse
    /// <summary>
    /// 将<see cref="HttpResponseMessage"/>转换为<see cref="HttpResponse"/>
    /// </summary>
    /// <param name="message">待转换的<see cref="HttpResponseMessage"/></param>
    /// <returns></returns>
    internal async static Task<HttpResponse> ToHttpResponse(this HttpResponseMessage message)
        => new HttpResponse()
        {
            Status = message.StatusCode,
            RequestUri = message.RequestMessage?.RequestUri?.AbsoluteUri ??
            $"这条消息不是通过{nameof(IHttpClient)}发送的，所以无法获取请求地址",
            Header = new HttpHeaderResponse(message.Headers),
            Content = (await message.Content.ToHttpContent())!
        };
    #endregion
    #endregion
    #endregion
    #region 有关字符串互相转换
    #region 转换Base64字符串
    #region 转换为Base64字符串
    /// <summary>
    /// 将字符串转换为Base64字符串，
    /// 它替换了所有不安全的字符
    /// </summary>
    /// <param name="text">封装要转换的字符串的对象</param>
    /// <returns></returns>
    public static string ToBase64(this StringOperate text)
    {
        var plainTextBytes = text.Text.ToBytes();
        var base64 = Convert.ToBase64String(plainTextBytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        return base64;
    }
    #endregion
    #region 从Base64字符串转换
    /// <summary>
    /// 将Base64字符串解码，然后将其转换为字符串
    /// </summary>
    /// <param name="text">封装要转换的字符串的对象</param>
    /// <returns></returns>
    public static string FromBase64(this StringOperate text)
    {
        var secureUrlBase64 = text.Text.Replace('-', '+').Replace('_', '/');
        switch (secureUrlBase64.Length % 4)
        {
            case 2:
                secureUrlBase64 += "==";
                break;
            case 3:
                secureUrlBase64 += "=";
                break;
        }
        var bytes = Convert.FromBase64String(secureUrlBase64);
        return Encoding.UTF8.GetString(bytes);
    }
    #endregion
    #endregion
    #region 转换Hex字符串
    #region 转换为Hex字符串
    /// <summary>
    /// 将字符串转换为Hex字符串，
    /// 例如%FF%AA%BB的形式
    /// </summary>
    /// <param name="text">封装待转换的字符串的对象</param>
    /// <returns></returns>
    public static string ToHex(this StringOperate text)
    {
        var hex = Convert.ToHexString(text.Text.ToBytes());
        var result = hex.Chunk(2).Join(x => $"{x[0]}{x[1]}", "%");
        return result.IsVoid() ? "" : "%" + result;
    }
    #endregion
    #endregion
    #region 获取终结点
    /// <summary>
    /// 获取Uri路径的终结点，
    /// 它也可以用于获取Uri中静态文件的名称
    /// </summary>
    /// <param name="text">封装要获取终结点的Uri的对象</param>
    /// <returns></returns>
    public static string GetEndPoint(this StringOperate text)
    {
        var uri = text.Text;
        var index = uri.LastIndexOf('/');
        return index == -1 ? "" : uri[(index + 1)..];
    }
    #endregion
    #region 有关路径转换
    #region 说明文档
    /*问：本地路径和Uri路径有什么区别？
      答：本地路径是在服务器本机上的路径，它以左斜杠作为分隔符，
      Uri路径是客户端从服务端获取资源的路径，它以右斜杠作为分隔符*/
    #endregion
    #region Uri路径和本地路径之间的转换
    #region 将Uri路径转换为本地路径
    /// <summary>
    /// 将Uri路径转换为本地路径
    /// </summary>
    /// <param name="localPath">封装待转换的Uri路径的对象</param>
    /// <param name="addWwwRoot">如果这个值为<see langword="true"/>，
    /// 则会在路径前面添加wwwroot文件夹，使这个路径可以直接通过与IO有关的API进行访问</param>
    /// <returns></returns>
    public static string ToLocalPath(this StringOperate localPath, bool addWwwRoot = false)
    {
        var path = (Uri.TryCreate(localPath.Text, UriKind.Absolute, out var url) ?
            url.LocalPath : localPath.Text.Replace('/', '\\')).TrimStart('\\');
        return addWwwRoot ? Path.Combine("wwwroot", path) : path;
    }
    #endregion
    #region 将本地转换为Uri路径
    /// <summary>
    /// 将本地转换为Uri路径
    /// </summary>
    /// <param name="localPath">封装待转换的本地路径的对象</param>
    /// <returns></returns>
    public static string ToUriPath(this StringOperate localPath)
        => localPath.ToVirtualPath().Replace("\\", "/");
    #endregion
    #endregion
    #region 真实路径和虚拟路径之间的转换
    #region 将真实路径转换为虚拟路径
    /// <summary>
    /// 将真实的物理路径转换为相对wwwroot文件夹的虚拟路径
    /// </summary>
    /// <param name="localPath">封装待转换的真实路径的对象</param>
    /// <returns></returns>
    public static string ToVirtualPath(this StringOperate localPath)
    {
        var text = localPath.Text;
        const string wwwroot = "wwwroot";
        var index = text.IndexOf("wwwroot");
        return index < 0 ?
            text :
            text[(index + wwwroot.Length)..];
    }
    #endregion
    #region 将虚拟路径转换为真实路径
    /// <summary>
    /// 将相对于wwwroot文件夹的虚拟路径转换为真实路径
    /// </summary>
    /// <param name="realityPath">封装待转换的虚拟路径的对象</param>
    /// <returns></returns>
    public static string ToRealityPath(this StringOperate realityPath)
        => Path.Combine("wwwroot", realityPath.Text.TrimStart('\\'));
    #endregion
    #endregion
    #region Web编码
    /// <summary>
    /// 将路径编码为兼容Web的格式
    /// </summary>
    /// <param name="path">要编码的路径</param>
    /// <returns></returns>
    public static string ToWebEncode(this StringOperate path)
    {
        #region 本地函数
        static string Fun(string path, char delimiter, Func<string, string>? recursion)
        {
            var split = path.Split(delimiter);
            var part = split.Select(x => recursion?.Invoke(x) ?? WebUtility.UrlEncode(x).Replace("+", "%20")).ToArray();
            return part.Join(delimiter.ToString());
        }
        #endregion
        return Fun(path.Text, '/',
            x => Fun(x, '?',
            x => Fun(x, '#', null)));
    }
    #endregion
    #endregion
    #endregion
    #region 关于依赖注入
    #region 注入IHttpClient
    /// <summary>
    /// 以瞬间模式注入一个<see cref="IHttpClient"/>，
    /// 它可以用来发起Http请求，
    /// 如果服务容器中注册了<see cref="HttpRequestTransform"/>，
    /// 那么还会将它作为<see cref="IHttpClient"/>的请求转换函数
    /// </summary>
    /// <param name="services">要注入的服务集合</param>
    /// <returns></returns>
    public static IServiceCollection AddIHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient();
        return services.AddTransient(x =>
        {
            var requestTransform = x.GetService<HttpRequestTransform>();
            return x.GetRequiredService<IHttpClientFactory>().CreateClient().ToHttpClient(requestTransform);
        });
    }
    #endregion 
    #region 注入SignalRProvide对象
    /// <summary>
    /// 以瞬间模式注入一个<see cref="ISignalRProvide"/>对象，
    /// 该依赖注入能够自动处理绝对路径和相对路径的转换，
    /// 它依赖于<see cref="IUriManager"/>服务
    /// </summary>
    /// <param name="services">待注入的容器</param>
    /// <param name="create">该委托传入中心的绝对Uri，以及一个用来提供服务的对象，
    /// 然后创建一个新的<see cref="HubConnection"/>，如果为<see langword="null"/>，则使用默认方法</param>
    /// <returns></returns>
    public static IServiceCollection AddSignalRProvide(this IServiceCollection services, Func<string, IServiceProvider, Task<HubConnection>>? create = null)
        => services.AddTransient(server =>
        {
            var navigation = server.GetRequiredService<IUriManager>();
            return CreateNet.SignalRProvide(create is null ? null : uri => create(uri, server),
                uri => navigation.Convert(uri, true));
        });
    #endregion
    #endregion 
}
