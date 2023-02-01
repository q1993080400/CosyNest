﻿using System.Net.Http.Headers;
using System.NetFrancis;
using System.NetFrancis.Http;
using System.Text;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace System;

/// <summary>
/// 有关网络的扩展方法全部放在这里
/// </summary>
public static class ExtenNet
{
    #region 有关IHttpClient及其衍生类型
    #region 公开成员
    #region 将HttpClient转换为IHttpClient
    /// <summary>
    /// 返回一个<see cref="HttpClient"/>的<see cref="IHttpClient"/>包装
    /// </summary>
    /// <param name="httpClient">待包装的<see cref="HttpClient"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="HttpClientRealize(HttpClient, bool)"/>
    public static IHttpClient ToHttpClient(this HttpClient httpClient, bool throwException = true)
        => new HttpClientRealize(httpClient, throwException);
    #endregion
    #region 读取IHttpResponse的内容，并将其转换为更高层的类型
    /// <summary>
    /// 读取一个<see cref="IHttpResponse"/>的内容，
    /// 并将其转换为更高层次的类型返回，
    /// 如果请求未能成功，则抛出一个异常
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="response">待读取的<see cref="IHttpResponse"/></param>
    /// <param name="read">这个委托传入待读取的<see cref="IHttpContent"/>，返回转换后的类型</param>
    /// <returns></returns>
    public static async Task<Ret> Read<Ret>(this Task<IHttpResponse> response, Func<IHttpContent, Task<Ret>> read)
    {
        var r = await response;
        r.ThrowIfNotSuccess();
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
        var stream = new MemoryStream();
        await content.CopyToAsync(stream);
        return new HttpContentRecording()
        {
            Content = stream.ToBitPipe().Read,
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
        var arryContent = new ByteArrayContent(array);
        content.Header.CopyHeader(arryContent.Headers);
        return arryContent;
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
    #region 转换Uri字符串
    #region 转换为Uri字符串
    /// <summary>
    /// 将字符串转换为Uri字符串，
    /// 它替换了所有不安全的字符
    /// </summary>
    /// <param name="text">封装要转换的字符串的对象</param>
    /// <returns></returns>
    public static string ToUriText(this StringOperate text)
    {
        var plainTextBytes = text.Text.ToBytes();
        var base64 = Convert.ToBase64String(plainTextBytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        return base64;
    }
    #endregion
    #region 从Uri字符串转换
    /// <summary>
    /// 将Uri字符串解码，然后将其转换为字符串
    /// </summary>
    /// <param name="text">封装要转换的字符串的对象</param>
    /// <returns></returns>
    public static string FromUriText(this StringOperate text)
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
    #region 将真实路径转换为虚拟路径
    /// <summary>
    /// 将真实的物理路径转换为相对wwwroot文件夹的虚拟路径
    /// </summary>
    /// <param name="localPath">封装待转换的真实路径的对象</param>
    /// <returns></returns>
    public static string ToVirtualPath(this StringOperate localPath)
    {
        var split = localPath.Text.Split("wwwroot");
        return split.Length == 1 ? split[0] : split[^1];
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
    #endregion
    #region 关于依赖注入
    #region 注入IHttpClient
    #region 不指定基地址
    /// <summary>
    /// 注入一个<see cref="IHttpClient"/>，
    /// 它可以用来发起Http请求
    /// </summary>
    /// <param name="services">要注入的服务集合</param>
    /// <returns></returns>
    public static IServiceCollection AddIHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient();
        return services.AddTransient(x => x.GetRequiredService<IHttpClientFactory>().CreateClient().ToHttpClient());
    }
    #endregion
    #region 指定基地址
    /// <summary>
    /// 注入一个<see cref="IHttpClient"/>，
    /// 它可以用于请求WebApi，且支持通过相对地址请求
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <param name="getBaseAddress">用来获取请求基地址的委托，
    /// 基地址通常是服务器的域名</param>
    /// <returns></returns>
    public static IServiceCollection AddIHttpClientHasAddress(this IServiceCollection services, Func<IServiceProvider, string> getBaseAddress)
    {
        services.AddHttpClient("webapi");
        services.AddScoped(server =>
        {
            var http = server.GetRequiredService<IHttpClientFactory>().CreateClient("webapi");
            var uri = getBaseAddress(server);
            http.BaseAddress = new(uri);
            return http.ToHttpClient();
        });
        return services;
    }
    #endregion
    #region 指定基地址，依赖于IUriManager
    /// <summary>
    /// 注入一个<see cref="IHttpClient"/>，
    /// 它可以用于请求WebApi，且支持通过相对地址请求，
    /// 它依赖于服务<see cref="IUriManager"/>
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddIHttpClientHasAddress(this IServiceCollection services)
        => services.AddIHttpClientHasAddress
        (x => x.GetRequiredService<IUriManager>().Uri.UriHost ??
        throw new NullReferenceException($"{nameof(IUriManager)}对象返回的是相对Uri，它没有主机部分"));
    #endregion
    #endregion 
    #region 注入SignalRProvide对象
    /// <summary>
    /// 以瞬间模式注入一个<see cref="ISignalRProvide"/>对象，
    /// 该依赖注入能够自动处理绝对路径和相对路径的转换，
    /// 它依赖于<see cref="IUriManager"/>服务
    /// </summary>
    /// <param name="services">待注入的容器</param>
    /// <returns></returns>
    /// <inheritdoc cref="CreateNet.SignalRProvide(Func{string, HubConnection}?, Func{string, string}?)"/>
    public static IServiceCollection AddISignalRProvide(this IServiceCollection services, Func<string, HubConnection>? create = null)
        => services.AddTransient(server =>
        {
            var navigation = server.GetRequiredService<IUriManager>();
            return CreateNet.SignalRProvide(create, uri => navigation.Convert(uri, true));
        });
    #endregion
    #region 注入HttpRequestTransformation
    /// <summary>
    /// 以范围模式注入一个<see cref="HttpRequestHelp"/>，
    /// 它可以封装一个Http转换函数
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddHttpRequestTransformation(this IServiceCollection services)
        => services.AddScoped<HttpRequestHelp>();
    #endregion
    #endregion 
}
