using System.Design;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;

namespace System;

/// <summary>
/// 有关安全的扩展方法全部放在这里
/// </summary>
public static class ExtenSafety
{
    #region 关于IPrincipal
    #region 返回主体是否被验证
    /// <summary>
    /// 返回主体是否被验证
    /// </summary>
    /// <param name="principal">待检查验证状态的主体，
    /// 如果为<see langword="null"/>，则直接返回<see langword="false"/></param>
    /// <returns></returns>
    public static bool IsAuthenticated([NotNullWhen(true)] this IPrincipal? principal)
        => principal is { Identity.IsAuthenticated: true };
    #endregion
    #endregion
    #region 关于ClaimsPrincipal
    #region 以ClaimsIdentity的形式返回主标识
    /// <summary>
    /// 以<see cref="ClaimsIdentity"/>的形式返回主声明标识
    /// </summary>
    /// <param name="principal">要返回声明标识的主体</param>
    /// <returns></returns>
    public static ClaimsIdentity Identity(this ClaimsPrincipal principal)
        => principal.Identity switch
        {
            null => throw new NullReferenceException($"{nameof(principal.Identity)}为null"),
            ClaimsIdentity identity => identity,
            var identity => throw new NotSupportedException($"{nameof(principal.Identity)}是{identity.GetType()}类型，" +
                $"无法转换为{nameof(ClaimsIdentity)}")
        };
    #endregion
    #region 关于标识Json对象
    #region 获取ClaimsIdentity中储存的对象
    /// <summary>
    /// 获取<see cref="ClaimsIdentity"/>中储存的对象
    /// </summary>
    /// <typeparam name="Obj">储存的对象的类型</typeparam>
    /// <param name="identity">储存对象的<see cref="ClaimsIdentity"/></param>
    /// <param name="type">对象的类型，即用来索引对象的键</param>
    /// <param name="options">一个用于控制Json转换的对象</param>
    /// <returns></returns>
    public static Obj? GetInfo<Obj>(this ClaimsIdentity identity, string type, JsonSerializerOptions? options = null)
    {
        var first = identity.FindFirst(type);
        return first is null ?
            default :
            JsonSerializer.Deserialize<Obj>(first.Value, options ?? CreateDesign.JsonCommonOptions);
    }
    #endregion
    #region 写入ClaimsIdentity中储存的对象
    /// <summary>
    /// 写入<see cref="ClaimsIdentity"/>中储存的对象
    /// </summary>
    /// <param name="obj">待写入的对象，
    /// 如果写入<see langword="null"/>，表示移除对象</param>
    /// <inheritdoc cref="GetInfo{Obj}(ClaimsIdentity, string, JsonSerializerOptions?)"/>
    public static void SetInfo<Obj>(this ClaimsIdentity identity, string type, Obj? obj, JsonSerializerOptions? options = null)
    {
        var first = identity.FindFirst(type);
        if (first is { })
            identity.RemoveClaim(first);
        if (obj is null)
            return;
        var json = JsonSerializer.Serialize(obj, options ?? CreateDesign.JsonCommonOptions);
        identity.AddClaim(new(type, json));
    }
    #endregion
    #endregion 
    #endregion
}
