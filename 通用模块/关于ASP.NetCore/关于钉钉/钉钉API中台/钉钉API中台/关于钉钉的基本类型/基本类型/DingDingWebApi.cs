using System.NetFrancis.Api;
using System.NetFrancis.Http;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型是钉钉WebAPI的基类
/// </summary>
/// <param name="info">用来创建配置的参数</param>
public abstract class DingDingWebApi
    (DingDingWebApiInfo info) :
    WebApi(() => info.ServiceProvider.GetRequiredService<IHttpClient>())
{
    #region 用来请求服务的对象
    /// <summary>
    /// 获取一个用来请求服务的对象
    /// </summary>
    protected IServiceProvider ServiceProvider { get; } = info.ServiceProvider;
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
    /// 获取公司的Token，
    /// 如果获取失败，则为<see langword="null"/>
    /// </summary>
    /// <returns></returns>
    protected async Task<string?> GetCompanyToken()
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
        return response.GetValueOrDefault("accessToken")?.ToString();
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
}
