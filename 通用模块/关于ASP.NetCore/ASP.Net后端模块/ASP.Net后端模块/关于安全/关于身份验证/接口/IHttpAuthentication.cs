using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以对<see cref="HttpContext"/>进行身份验证
    /// </summary>
    public interface IHttpAuthentication
    {
        #region 本框架的推荐身份验证方式
        /*说明文档
          问：本框架推荐使用什么身份验证方式？
          答：推荐使用哈希值验证

          问：什么是哈希值验证？
          答：它是对密码加盐的进一步拓展，验证步骤如下：
          1.将用户名和密码拼接起来，计算它的哈希值，得到结果h
          2.检查数据库（或其他存储位置）中是否存在h，
          如果存在，则验证通过，反之则验证不通过
          由于任何人获得此哈希值都可以伪造身份，因此请务必使用Https协议
         
          问：这种做法有什么好处？
          答：好处如下：
          1.自动实现加盐，无需专门流程
          2.数据库不需要储存用户名，只需要储存哈希值和一个唯一ID，
          如果泄露，用户名和密码都不会暴露
          3.前端传入身份验证信息时，不需要传入用户名和密码，只需要传入哈希值，
          数据库验证时，不需要检查用户名和密码是否匹配，只需要检查哈希值是否存在，比较方便
        
          问：在这种情况下，用于登录的API应该怎么设计？
          答：推荐做法是，由于ExtenWebApi.UseAuthenticationFrancis中间件在任何请求处理前都会进行身份验证，
          因此实际上根本不需要登陆这个功能，用于"登录"的API的实际作用就是读取HttpContext.User属性，
          并返回一个Json，告知客户端验证是否通过，以及如果验证不通过的原因，这样做的好处如下：
          
          1.极度简化，API甚至不需要任何参数，因为HttpContext就是它的参数，也不需要使用POST请求
          2.每次请求都会进行验证，提高了安全性
          3.验证结果不会被记住，无状态，便于扩展，
          如果验证通过，前端自行保存验证信息，并在下次请求时带上
          4.不需要声明有关注销的API，因为前端不附上验证信息等同注销
          5.结合哈希值验证，验证信息不需要数字签名，因为只有知道用户名和密码才能够伪造它，
          同时也不需要过期机制，因为可以通过修改数据库中的哈希值来撤销验证，这是相对于Token的优点
        
          问：在这种设计下，每次请求都会有一次额外的访问数据库操作，请问这样是否值得？
          答：软件工程的业务逻辑发展到今天已经非常复杂，每次请求必然会产生大量的数据库访问，
          多一个不多，少一个不少*/
        #endregion
        #region 验证HttpContext
        /// <summary>
        /// 验证一个Http上下文，并返回验证结果，
        /// 如果验证通过，还会将结果写入<see cref="HttpContext.User"/>属性
        /// </summary>
        /// <param name="context">需要验证的Http上下文</param>
        /// <returns></returns>
        Task<ClaimsPrincipal> Verify(HttpContext context);

        /*实现本API请遵循以下规范：
          #从HttpContext中获取身份验证信息，
          包括但不限于令牌，用户名密码等，应该至少同时支持两种方式：
          Cookie以及Http请求的Authentication标头，因为作者注意到，
          在浏览器发起的请求中，使用前者携带验证信息比较方便，
          直接通过API发起的请求，例如HttpClient，使用后者比较方便，
          本API不应该假定请求来自何处，因此需要同时兼顾这两种方式*/
        #endregion
        #region 写入验证结果
        /// <summary>
        /// 将验证结果写入Http上下文，使它能够被持久化
        /// </summary>
        /// <param name="credentials">待写入的验证结果</param>
        /// <param name="context">用来写入验证结果的Http上下文</param>
        Task SetVerify(ClaimsPrincipal credentials, HttpContext context);
        #endregion
    }
}
