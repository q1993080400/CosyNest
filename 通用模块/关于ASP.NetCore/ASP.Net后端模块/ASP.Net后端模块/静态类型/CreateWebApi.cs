using System.Security.Claims;
using System.Text.Json;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型可以用来创建有关WebApi的对象
/// </summary>
public static class CreateWebApi
{
    #region 有关Json
    #region 创建Json格式化器
    #region 输出Json
    /// <summary>
    /// 创建一个<see cref="TextOutputFormatter"/>，
    /// 它可以将受支持的类型序列化为Json并在WebApi中返回
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="JsonOutputFormatterGeneral(JsonSerializerOptions)"/>
    public static TextOutputFormatter OutputFormatterJson(JsonSerializerOptions options)
        => new JsonOutputFormatterGeneral(options);
    #endregion
    #region 输入Json
    /// <summary>
    /// 创建一个<see cref="TextInputFormatter"/>，
    /// 它可以在WeiApi中接受Json，并将其反序列化为受支持的类型
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="JsonInputFormatterGeneral(JsonSerializerOptions)"/>
    public static TextInputFormatter InputFormatterJson(JsonSerializerOptions options)
        => new JsonInputFormatterGeneral(options);
    #endregion
    #endregion
    #endregion
    #region 创建HttpAuthentication
    #region 通过Cookies和Authentication标头进行验证
    #region 直接创建
    /// <summary>
    /// 创建一个<see cref="Authentication.HttpAuthentication"/>，
    /// 它从Cookies和Authentication标头中提取信息，并验证身份
    /// </summary>
    /// <param name="extraction"> 这个委托被用于从<see cref="HttpContext"/>中提取身份验证信息，
    /// 如果不存在身份验证信息，则返回<see langword="null"/></param>
    /// <param name="authentication">这个委托的参数是提取到的验证信息，返回值是验证结果</param>
    /// <returns></returns>
    public static HttpAuthentication HttpAuthentication
        (Func<HttpContext, string?> extraction,
        Func<string, Task<ClaimsPrincipal>> authentication)
        => new HttpAuthenticationRealize(extraction, authentication).Authentication;
    #endregion
    #region 简单版本
    /// <param name="authenticationKey">用来从Cookies中提取身份验证信息的键名</param>
    /// <inheritdoc cref="HttpAuthentication(Func{HttpContext, string?}, Func{string, Task{ClaimsPrincipal}})"/>
    public static HttpAuthentication HttpAuthenticationSimple
        (Func<string, Task<ClaimsPrincipal>> authentication,
        string authenticationKey = ToolASP.AuthenticationKey)
        => HttpAuthentication(http =>
        http.Request.Headers.TryGetValue("Authentication", out var headers) && headers is [{ } h, ..] ?
        h.Split(" ")[1] :
        http.Request.Cookies[authenticationKey], authentication);
    #endregion
    #endregion
    #endregion
}
