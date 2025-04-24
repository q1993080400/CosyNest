namespace System.NetFrancis;

public static partial class CreateNet
{
    //这个部分类专门用来声明用来创建有关Http的对象的方法

    #region 从IHttpClientFactory中提取HttpClient的默认名称
    /// <summary>
    /// 获取一个从<see cref="IHttpClientFactory"/>中提取<see cref="HttpClient"/>的时候，
    /// 默认使用的名称
    /// </summary>
    public const string HttpClientName = "Default";
    #endregion
    #region 创建HttpRequestTransform
    #region 添加一个基路径
    /// <summary>
    /// 创建一个<see cref="HttpRequestTransform"/>，
    /// 如果当前请求没有指定基路径，
    /// 它可以自动为其添加一个
    /// </summary>
    /// <param name="baseUri">请求的基路径，它一般是应用的Host</param>
    /// <returns></returns>
    public static HttpRequestTransform TransformBaseUri(string baseUri)
        => (request) =>
        {
            var uri = request.Uri;
            var newRequest = uri.UriHost is { } ?
            request :
            request with
            {
                Uri = uri with
                {
                    UriHost = baseUri
                }
            };
            return Task.FromResult(newRequest);
        };
    #endregion 
    #endregion
}