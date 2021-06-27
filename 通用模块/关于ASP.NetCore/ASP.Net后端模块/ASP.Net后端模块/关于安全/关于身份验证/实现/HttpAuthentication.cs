using System;
using System.SafetyFrancis;
using System.SafetyFrancis.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication
{
    /// <summary>
    /// 这个类型是<see cref="IHttpAuthentication"/>的实现，
    /// 它从Cookies和Http请求的Authentication标头中提取信息，并验证身份
    /// </summary>
    class HttpAuthentication : IHttpAuthentication
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
        private AuthenticationFunction<string> Authentication { get; }
        #endregion
        #endregion
        #region 接口实现
        #region 验证HttpContext
        public async Task<ClaimsPrincipal> Verify(HttpContext context)
        {
            var token = Extraction(context);
            if (token is null)
                return CreateSafety.PrincipalDefault;
            var user = await Authentication(token);
            if (user.IsAuthenticated())
                context.User = user;
            return user;
        }
        #endregion
        #region 写入验证结果
        public Task SetVerify(ClaimsPrincipal credentials, HttpContext context)
            => Task.CompletedTask;
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Extraction"> 这个委托被用于从<see cref="HttpContext"/>中提取身份验证信息，
        /// 如果不存在身份验证信息，则返回<see langword="null"/></param>
        /// <param name="Authentication">这个委托可以通过验证信息来获取身份验证结果</param>
        public HttpAuthentication(Func<HttpContext, string?> Extraction, AuthenticationFunction<string> Authentication)
        {
            this.Extraction = Extraction;
            this.Authentication = Authentication;
        }
        #endregion
    }
}
