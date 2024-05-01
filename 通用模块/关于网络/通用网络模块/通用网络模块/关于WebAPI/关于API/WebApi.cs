using System.NetFrancis.Http;

namespace System.NetFrancis.Api;

/// <summary>
/// 本类型是所有WebApi的基类
/// </summary>
/// <param name="httpClientProvide">一个Http客户端提供程序，
/// 它提供用于发起请求的<see cref="IHttpClient"/>，
/// 如果为<see langword="null"/>，则使用<see cref="CreateNet.HttpClientShared"/></param>
public abstract class WebApi(Func<IHttpClient>? httpClientProvide)
{
    #region Http客户端提供程序
    /// <summary>
    /// 获取一个Http客户端提供程序，
    /// 它提供用于发起请求的<see cref="IHttpClient"/>
    /// </summary>
    protected Func<IHttpClient> HttpClientProvide { get; } = httpClientProvide ??= () => CreateNet.HttpClientShared;
    #endregion
    #region 无参数构造函数
    public WebApi()
        : this(null)
    {

    }
    #endregion
}
