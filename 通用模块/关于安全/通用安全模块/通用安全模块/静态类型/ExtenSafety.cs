using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Principal;

namespace System
{
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
            => principal is { Identity: { IsAuthenticated: true } };
        #endregion
        #endregion
        #region 关于ClaimsPrincipal
        #region 以ClaimsIdentity的形式返回主标识
        /// <summary>
        /// 以<see cref="ClaimsIdentity"/>的形式返回主声明标识
        /// </summary>
        /// <param name="principal">要返回声明标识的主体</param>
        /// <returns></returns>
        public static ClaimsIdentity? Identity(this ClaimsPrincipal principal)
            => (ClaimsIdentity?)principal.Identity;
        #endregion
        #endregion
        #region 关于ClaimsIdentity 
        #region 返回验证不通过的原因
        /// <summary>
        /// 返回标识验证不通过的原因，
        /// 如果标识验证通过，或者没有记载原因，
        /// 则返回<see langword="null"/>
        /// </summary>
        /// <param name="identity">待返回验证不通过原因的标识</param>
        /// <returns></returns>
        public static string? GetBanReason(this ClaimsIdentity identity)
            => !identity.IsAuthenticated && identity.FindFirst("BanReason") is { } c ? c.Value : null;
        #endregion
        #region 写入验证不通过的原因
        /// <summary>
        /// 写入标识验证不通过的原因，
        /// 如果标识已经验证通过，则不执行任何操作
        /// </summary>
        /// <param name="identity">待写入验证不通过原因的标识</param>
        /// <param name="reason">验证不通过的原因</param>
        /// <returns>原路返回<paramref name="identity"/></returns>
        public static ClaimsIdentity SetBanReason(this ClaimsIdentity identity, string reason)
        {
            if (!identity.IsAuthenticated)
                identity.AddClaim(new("BanReason", reason));
            return identity;
        }
        #endregion
        #endregion
    }
}
