
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace System
{
    public partial class ExtenWebApi
    {
        //这个部分类专门用来声明有关中间件的扩展方法

        #region 添加审阅中间件
        /// <summary>
        /// 添加一个审阅中间件，
        /// 它可以用来查看<see cref="HttpContext"/>对象，
        /// 没有其他的功能
        /// </summary>
        /// <param name="app">待添加中间件的<see cref="IApplicationBuilder"/>对象</param>
        /// <param name="review">用来查看<see cref="HttpContext"/>的委托</param>
        public static void UseReview(this IApplicationBuilder app, Action<HttpContext> review)
            => app.Use(async (context, next) =>
            {
                review(context);
                await next();
            });
        #endregion
        #region 添加验证中间件
        /// <summary>
        /// 添加一个身份验证中间件，
        /// 它依赖于服务<see cref="IHttpAuthentication"/>
        /// </summary>
        /// <param name="application">待添加中间件的<see cref="IApplicationBuilder"/></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuthenticationFrancis(this IApplicationBuilder application)
            => application.Use(static async (context, next) =>
            {
                var auth = context.RequestServices.GetRequiredService<IHttpAuthentication>();
                await auth.Verify(context);
                context.Response.OnStarting(() => auth.SetVerify(context.User, context));      //在后面的中间件全部执行完毕后，将验证结果写回响应中
                await next();
            });
        #endregion
    }
}
