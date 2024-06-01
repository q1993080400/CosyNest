using System.Design;
using System.NetFrancis.Api;
using System.NetFrancis.Http;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace System.DingDing;

/// <summary>
/// 这个类型是钉钉WebAPI的基类
/// </summary>
/// <param name="serviceProvider">用来请求服务的对象</param>
public abstract class DingDingWebApi
    (IServiceProvider serviceProvider) :
    WebApi(serviceProvider.GetRequiredService<IHttpClient>)
{
    #region 用来请求服务的对象
    /// <summary>
    /// 获取一个用来请求服务的对象
    /// </summary>
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;
    #endregion
    #region 获取用户Token
    /// <summary>
    /// 返回真正可用于身份验证的Token，
    /// 注意：它仅用于用户身份验证，不用于企业身份验证
    /// </summary>
    /// <param name="oldAuthenticationRequest">旧的身份验证票证</param>
    /// <returns></returns>
    protected async Task<AuthenticationDingDingRequest?> GetUserToken(AuthenticationDingDingRequest oldAuthenticationRequest)
    {
        var configuration = ServiceProvider.GetRequiredService<DingDingConfiguration>();
        var http = ServiceProvider.GetRequiredService<IHttpClient>();
        var dataProtection = GetDataProtection();
        object httpParameter = oldAuthenticationRequest switch
        {
            { IsToken: true, RefreshToken: { } refreshToken } =>
            new
            {
                clientId = configuration.ClientID,
                refreshToken = dataProtection.Unprotect(oldAuthenticationRequest.RefreshToken),
                grantType = "refresh_token",
                clientSecret = configuration.ClientSecret
            },
            { IsToken: false, RefreshToken: null } =>
            new
            {
                clientId = configuration.ClientID,
                code = oldAuthenticationRequest.Code,
                grantType = "authorization_code",
                clientSecret = configuration.ClientSecret
            },
            _ => throw new NotSupportedException($"{nameof(AuthenticationDingDingRequest)}对象的配置不正确")
        };
        var uri = "https://api.dingtalk.com/v1.0/oauth2/userAccessToken";
        var response = await http.RequestPost(uri, httpParameter).
                Read(x => x.ToObject());
        var access = response.GetValueOrDefault("accessToken")?.ToString();
        var refresh = response.GetValueOrDefault("refreshToken")?.ToString();
        if (access is null || refresh is null)
            return null;
        var newAuthenticationRequest = new AuthenticationDingDingRequest()
        {
            Code = access,
            RefreshToken = refresh,
            IsToken = true
        };
        return newAuthenticationRequest;
    }
    #endregion
    #region 获取公司Token
    /// <summary>
    /// 获取公司的Token
    /// </summary>
    /// <returns></returns>
    protected async Task<string> GetCompanyToken()
    {
        var configuration = ServiceProvider.GetRequiredService<DingDingConfiguration>();
        var http = ServiceProvider.GetRequiredService<IHttpClient>();
        var info = new
        {
            appKey = configuration.ClientID,
            appSecret = configuration.ClientSecret
        };
        var response = await http.RequestPost("https://api.dingtalk.com/v1.0/oauth2/accessToken", info).
            Read(x => x.ToObject());
        return response.GetValueOrDefault("accessToken")?.ToString() ??
            throw new NotSupportedException("无法获取访问令牌");
    }
    #endregion
    #region 获取加密对象
    /// <summary>
    /// 获取一个钉钉加密对象
    /// </summary>
    /// <returns></returns>
    protected IDataProtector GetDataProtection()
    {
        var dataProtectionProvider = ServiceProvider.GetRequiredService<IDataProtectionProvider>();
        return dataProtectionProvider.CreateProtector("DingDing");
    }
    #endregion
    #region 用来添加访问令牌的转换函数
    /// <summary>
    /// 返回一个转换函数，它将访问令牌添加到请求中
    /// </summary>
    /// <param name="token">要添加的访问令牌</param>
    /// <returns></returns>
    protected static HttpRequestTransform TransformAccessToken(string token)
        => x => x with
        {
            Header = x.Header.With(x => x.Add("x-acs-dingtalk-access-token", [token]))
        };
    #endregion
    #region 限流算法
    #region 用来限流的委托
    /// <summary>
    /// 这个委托使用令牌桶算法对API调用进行限流
    /// </summary>
    private IBlock Block { get; }
        = serviceProvider.GetRequiredService<IBlock>();
    #endregion
    #region 阻塞API调用
    /// <summary>
    /// 返回一个<see cref="Task"/>，
    /// 它可以用来对WebAPI的调用进行限流，
    /// 避免因为频繁请求而发生异常
    /// </summary>
    /// <returns></returns>
    protected Task Delay()
        => Block.Block().Wait;
    #endregion
    #endregion
}
