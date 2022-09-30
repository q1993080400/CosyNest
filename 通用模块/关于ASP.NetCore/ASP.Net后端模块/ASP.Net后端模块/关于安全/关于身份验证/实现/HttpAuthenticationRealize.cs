using System.SafetyFrancis;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication;

/// <summary>
/// 这个类型可以用暴露<see cref="HttpAuthentication"/>委托，
/// 它从Cookies和Http请求的Authentication标头中提取信息，并验证身份
/// </summary>
sealed class HttpAuthenticationRealize
{
    #region 封装的对象
    #region 用于提取身份验证信息的委托
    /// <summary>
    /// 这个委托被用于从<see cref="HttpContext"/>中提取身份验证信息，
    /// 如果不存在身份验证信息，则返回<see langword="null"/>
    /// </summary>
    private Func<HttpContext, string?> Extraction { get; }
    #endregion
    #region 用于验证的委托
    /// <summary>
    /// 这个委托可以通过验证信息来获取身份验证结果
    /// </summary>
    private Func<string, Task<ClaimsPrincipal>> Verify { get; }
    #endregion
    #endregion
    #region 接口实现
    #region 验证HttpContext
    /// <inheritdoc cref="HttpAuthentication"/>
    public async Task Authentication(HttpContext context)
    {
        context.User = Extraction(context) is { } t ?
            await Verify(t) : CreateSafety.PrincipalDefault;
    }
    #endregion
    #endregion
    #region 构造函数
    /// <inheritdoc cref="CreateWebApi.HttpAuthentication(Func{HttpContext, string?}, Func{string, Task{ClaimsPrincipal}})"/>
    public HttpAuthenticationRealize(Func<HttpContext, string?> extraction, Func<string, Task<ClaimsPrincipal>> authentication)
    {
        this.Extraction = extraction;
        this.Verify = authentication;
    }
    #endregion
}
