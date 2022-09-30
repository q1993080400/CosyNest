using System.Net.Http.Headers;
using System.NetFrancis.Http;
using System.Text;

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
    #endregion
    #region 内部成员
    #region 将HttpRequestRecording转换为HttpRequestMessage
    /// <summary>
    /// 将<see cref="HttpRequestRecording"/>转换为等效的<see cref="HttpRequestMessage"/>
    /// </summary>
    /// <param name="recording">待转换的<see cref="HttpRequestRecording"/></param>
    /// <param name="baseAddress">请求目标Uri的基地址</param>
    /// <returns></returns>
    internal static HttpRequestMessage ToHttpRequestMessage(this IHttpRequest recording, Uri? baseAddress)
    {
        var uri = recording.UriComplete;
        var m = new HttpRequestMessage()
        {
            RequestUri = baseAddress is null ? new(uri) : new(baseAddress, uri),
            Method = recording.HttpMethod,
            Content = recording.Content.ToHttpContent(),
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
    private static HttpContent? ToHttpContent(this IHttpContent? content)
    {
        if (content is null)
            return null;
        var arryContent = new ByteArrayContent(content.Content.ReadComplete().Result);
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
        foreach (var (key, value) in header.Headers())
        {
            bclHeader.Add(key, value);
        }
    }
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
}
