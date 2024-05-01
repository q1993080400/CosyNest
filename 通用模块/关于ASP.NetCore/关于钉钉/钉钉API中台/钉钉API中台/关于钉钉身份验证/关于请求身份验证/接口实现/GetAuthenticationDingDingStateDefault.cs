using System.NetFrancis.Http;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore;

/// <summary>
/// 本类型是<see cref="IGetAuthenticationDingDingState"/>的实现，
/// 可以用来返回钉钉身份验证的状态
/// </summary>
/// <inheritdoc cref="DingDingWebApi(DingDingWebApiInfo)"/>
sealed class GetAuthenticationDingDingStateDefault(DingDingWebApiInfo info) :
    DingDingWebApi(info), IGetAuthenticationDingDingState
{
    #region 返回钉钉身份验证状态
    public async Task<APIPackDingDing> GetAuthenticationDingDingState(AuthenticationDingDingRequest parameter)
    {
        var http = ServiceProvider.GetRequiredService<IHttpClient>();
        var configuration = ServiceProvider.GetRequiredService<DingDingConfiguration>();
        var dataProtection = GetDataProtection();
        var token = await GetUserToken(parameter);
        if (token is null or { IsToken: false } or { RefreshToken: null })
            return new()
            {
                AuthorizationState = new()
                {
                    AuthenticationState = new()
                    {
                        UserInfo = null,
                        NextRequest = parameter
                    },
                    AuthorizationPassed = false
                }
            };
        var (accessToken, refreshToken) = (token.Code, token.RefreshToken);
        var uri = $"https://api.dingtalk.com/v1.0/contact/users/me";
        var transform = ServiceProvider.GetService<HttpRequestTransform>();
        var response = await http.Request(uri, transformation:
            x => x with
            {
                Header = x.Header.With(x => x.Add("x-acs-dingtalk-access-token", [accessToken]))
            }).Read(x => x.ToObject());
        return new()
        {
            AuthorizationState = new()
            {
                AuthenticationState = new()
                {
                    UserInfo = new()
                    {
                        Name = response["nick"]?.ToString() ?? ""
                    },
                    NextRequest = new()
                    {
                        Code = dataProtection.Protect(accessToken),
                        IsToken = true,
                        RefreshToken = dataProtection.Protect(refreshToken),
                    }
                },
                AuthorizationPassed = true,
            }
        };
    }
    #endregion
    #region 获取App信息
    public Task<DingDingAppInfo> GetAppInfo()
    {
        var configuration = ServiceProvider.GetRequiredService<DingDingConfiguration>();
        return new DingDingAppInfo()
        {
            ClientID = configuration.ClientID,
            OrganizationID = configuration.OrganizationID
        }.ToTask();
    }
    #endregion
}
