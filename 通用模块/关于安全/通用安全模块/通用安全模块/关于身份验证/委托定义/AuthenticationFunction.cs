using System.Security.Claims;
using System.Threading.Tasks;

namespace System.SafetyFrancis.Authentication
{
    /// <summary>
    /// 这个委托可以用来执行身份验证
    /// </summary>
    /// <typeparam name="Parameters">用来验证身份的参数类型</typeparam>
    /// <param name="parameters">用来验证身份的参数</param>
    /// <returns></returns>
    public delegate Task<ClaimsPrincipal> AuthenticationFunction<in Parameters>(Parameters parameters);
}
