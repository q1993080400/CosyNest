using System.Net;
using System.NetFrancis;
using System.NetFrancis.Http;
using System.Text;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System;

/// <summary>
/// 有关网络的扩展方法全部放在这里
/// </summary>
public static class ExtendNet
{
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
    #region 注入HttpRequestTransform
    /// <summary>
    /// 以单例模式注入一个<see cref="HttpRequestTransform"/>，
    /// 它可以自动处理相对请求路径，将其视为请求本站，
    /// 本服务依赖于<see cref="IHostProvide"/>
    /// </summary>
    /// <param name="services">要注入的服务集合</param>
    /// <returns></returns>
    public static IServiceCollection AddHttpRequestTransformUri(this IServiceCollection services)
    {
        services.AddSingleton(x =>
        {
            var hostProvide = x.GetRequiredService<IHostProvide>();
            return CreateNet.TransformBaseUri(hostProvide.Host);
        });
        return services;
    }
    #endregion
    #region 注入SignalRProvide对象
    /// <summary>
    /// 以瞬间模式注入一个<see cref="ISignalRProvide"/>对象，
    /// 该依赖注入能够自动处理绝对路径和相对路径的转换，
    /// 它依赖于<see cref="IHostProvide"/>服务
    /// </summary>
    /// <param name="services">待注入的容器</param>
    /// <param name="create">该委托传入中心的绝对Uri，以及一个用来提供服务的对象，
    /// 然后创建一个新的<see cref="IHubConnectionBuilder"/>，如果为<see langword="null"/>，则使用默认方法</param>
    /// <returns></returns>
    public static IServiceCollection AddSignalRProvide(this IServiceCollection services, Func<string, IServiceProvider, Task<IHubConnectionBuilder>>? create = null)
        => services.AddTransient(server =>
        {
            var navigation = server.GetRequiredService<IHostProvide>();
            return CreateNet.SignalRProvide(create is null ? null : uri => create(uri, server),
                uri => navigation.Convert(uri, true));
        });
    #endregion
    #region 注入IHostProvide
    #region 直接指定Host
    /// <summary>
    /// 以单例模式注入一个<see cref="IHostProvide"/>，
    /// 它可以用于提供本机Host地址
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <param name="baseUri">本地主机的Host地址</param>
    /// <returns></returns>
    public static IServiceCollection AddHostProvide(this IServiceCollection services, string baseUri)
        => services.AddSingleton(_ => CreateNet.HostProvide(baseUri));
    #endregion
    #region 从配置中读取
    /// <summary>
    /// 以单例模式注入一个<see cref="IHostProvide"/>，
    /// 它可以用于提供本机Host地址，
    /// 它通过配置文件中一个叫Host的键提取地址
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddHostProvideFromConfiguration(this IServiceCollection services)
        => services.AddSingleton(services =>
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            var baseUri = configuration.GetValue<string>("Host") ??
            throw new NotSupportedException("没有在配置中设置主机的Host");
            return CreateNet.HostProvide(baseUri);
        });
    #endregion
    #endregion
    #endregion 
}
