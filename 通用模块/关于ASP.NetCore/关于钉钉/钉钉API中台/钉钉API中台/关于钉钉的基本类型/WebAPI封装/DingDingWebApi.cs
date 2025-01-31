using System.Design;
using System.Design.Direct;
using System.NetFrancis;
using System.NetFrancis.Api;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace System.DingDing;

/// <summary>
/// 这个类型是钉钉WebAPI的基类
/// </summary>
/// <param name="serviceProvider">用来请求服务的对象</param>
public abstract class DingDingWebApi
    (IServiceProvider serviceProvider) :
    WebApi(serviceProvider)
{
    #region 获取用户Token
    /// <summary>
    /// 返回真正可用于身份验证的Token，
    /// 注意：它仅用于用户身份验证，不用于企业身份验证
    /// </summary>
    /// <param name="authenticationRequest">旧的身份验证票证</param>
    /// <returns></returns>
    protected async Task<AuthenticationDingDingRequest?> GetUserToken(AuthenticationDingDingRequest authenticationRequest)
    {
        try
        {
            var dataProtector = GetDataProtection();
            var newAuthenticationRequest = authenticationRequest.Decryption(dataProtector);
            var configuration = ServiceProvider.GetRequiredService<DingDingConfiguration>();
            var http = HttpClient;
            object httpParameter = newAuthenticationRequest switch
            {
                { IsToken: true, RefreshToken: { } refreshToken } =>
                new
                {
                    clientId = configuration.ClientID,
                    refreshToken = newAuthenticationRequest.RefreshToken,
                    grantType = "refresh_token",
                    clientSecret = configuration.ClientSecret
                },
                { IsToken: false, RefreshToken: null } =>
                new
                {
                    clientId = configuration.ClientID,
                    code = newAuthenticationRequest.Code,
                    grantType = "authorization_code",
                    clientSecret = configuration.ClientSecret
                },
                _ => throw new NotSupportedException($"{nameof(AuthenticationDingDingRequest)}对象的配置不正确")
            };
            var uri = "https://api.dingtalk.com/v1.0/oauth2/userAccessToken";
            var response = await http.RequestJsonPost(uri, httpParameter);
            var access = response.GetValueOrDefault("accessToken")?.ToString();
            var refresh = response.GetValueOrDefault("refreshToken")?.ToString();
            if (access is null || refresh is null)
                return null;
            var returnAuthenticationRequest = new AuthenticationDingDingRequest()
            {
                Code = access,
                RefreshToken = refresh,
                IsToken = true,
                IsEncryption = false
            };
            return returnAuthenticationRequest;
        }
        catch (Exception ex)
        {
            ex.Log(ServiceProvider);
            return null;
        }
    }
    #endregion
    #region 获取加密对象
    /// <summary>
    /// 获取一个钉钉加密对象
    /// </summary>
    /// <param name="serviceProvider">服务请求对象</param>
    /// <returns></returns>
    protected IDataProtector GetDataProtection()
    {
        var dataProtectionProvider = ServiceProvider.GetRequiredService<IDataProtectionProvider>();
        return dataProtectionProvider.CreateProtector("DingDing");
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
        var http = HttpClient;
        var info = new
        {
            appKey = configuration.ClientID,
            appSecret = configuration.ClientSecret
        };
        var response = await http.RequestJsonPost("https://api.dingtalk.com/v1.0/oauth2/accessToken", info);
        return response.GetValueOrDefault("accessToken")?.ToString() ??
            throw new NotSupportedException("无法获取访问令牌");
    }
    #endregion
    #region 用来添加访问令牌的转换函数
    /// <summary>
    /// 返回一个转换函数，它将访问令牌添加到请求中
    /// </summary>
    /// <param name="token">要添加的访问令牌</param>
    /// <returns></returns>
    protected static Func<HttpRequestTransform, HttpRequestTransform> TransformAccessToken(string token)
        => transform => async request =>
        {
            var newRequest = await transform(request);
            return newRequest with
            {
                Header = newRequest.Header.With(x => x.Add("x-acs-dingtalk-access-token", [token]))
            };
        };
    #endregion
    #region 验证返回值
    /// <summary>
    /// 验证钉钉API的响应，
    /// 如果存在问题，则抛出异常，否则直接返回
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    protected static IDirect VerifyResponse(IDirect response)
    {
        if (response.TryGetValue("errormessage", out var errormessage))
            throw new NotSupportedException("请求钉钉API出现异常：" + errormessage?.ToString());
        return response;
    }
    #endregion
    #region 转换时间
    #region 返回不可空类型
    /// <summary>
    /// 钉钉返回的时间并没有携带时区信息，
    /// 调用本方法可以将时区信息转换为东8区
    /// </summary>
    /// <param name="dateTime">要转换的时间</param>
    /// <returns></returns>
    protected static DateTimeOffset ConvertDate(DateTimeOffset dateTime)
        => new(dateTime.DateTime, TimeSpan.FromHours(8));
    #endregion
    #region 返回可空类型
    /// <inheritdoc cref="ConvertDate(DateTimeOffset)"/>
    protected static DateTimeOffset? ConvertDate(DateTimeOffset? dateTime)
        => dateTime is null ? null : ConvertDate(dateTime.Value);
    #endregion
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
