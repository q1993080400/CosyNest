using System.Net;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace System;

public static partial class ExtenWebApi
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
    #region 重视异常中间件
    /// <summary>
    /// 添加一个重视异常中间件，
    /// 当处于非开发状态时，它不执行任何操作，
    /// 处于开发状态时，如果产生异常，它会中断整个服务器，方便寻找Bug
    /// </summary>
    /// <param name="application"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseImportanceException(this IApplicationBuilder application)
    {
        var environment = application.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        if (!environment.IsDevelopment())
            return application;
        application.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Use((HttpContext context, RequestDelegate _) =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature!.Error;
                throw new Exception("服务器发生错误，详情请参阅内部异常", exception);
            });
        });
        return application;
    }
    #endregion
    #region 添加身份验证中间件
    /// <summary>
    /// 添加一个身份验证中间件，
    /// 它依赖于服务<see cref="HttpAuthentication"/>
    /// </summary>
    /// <param name="application">待添加中间件的<see cref="IApplicationBuilder"/></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAuthenticationFrancis(this IApplicationBuilder application)
            => application.Use(static async (context, next) =>
            {
                await context.RequestServices.GetRequiredService<HttpAuthentication>()(context);
                await next();
            });
    #endregion
    #region 添加配置服务中间件
    /// <summary>
    /// 添加一个配置服务中间件，
    /// 它可以用来配置需要<see cref="HttpContext"/>，
    /// 且作用范围为Scoped的服务
    /// </summary>
    /// <typeparam name="Service">服务的类型</typeparam>
    /// <param name="application">待添加中间件的<see cref="IApplicationBuilder"/></param>
    /// <param name="configuration">用来配置服务的委托，
    /// 它的第一个参数是当前请求的<see cref="HttpContext"/>，第二个参数是请求到的服务</param>
    /// <returns></returns>
    public static IApplicationBuilder UseConfigurationService<Service>(this IApplicationBuilder application, Action<HttpContext, Service> configuration)
        where Service : class
        => application.Use((context, next) =>
         {
             configuration(context, context.RequestServices.GetRequiredService<Service>());
             return next();
         });

    /*问：该方法有什么意义？
      答：有些服务需要知道HttpContext，
      但是，Net规范不推荐直接使用IHttpContextAccessor来获取HttpContext，
      它会产生大量不可预测的问题，因此推荐做法是：
      注册一个生存期为范围的服务，使用一个中间件来获取HttpContext，
      并将服务所需要的数据复制过来

      但是需要注意以下问题：
      #不要在服务中直接保存HttpContext，这样一来本方法就失去了意义

      #Blazor为每个SignalR连接创建一个Scoped服务，
      但是本中间件仅为每个Http请求初始化服务，这会导致可能有的服务无法被初始化，
      作者会根据这个情况评估本方法的必要性，如证实这个问题非常严重，会删除本方法*/
    #endregion
    #region 添加访客统计中间件
    /// <summary>
    /// 添加一个访客统计中间件，
    /// 它可以用于统计登录或匿名的访客
    /// </summary>
    /// <param name="application">待添加中间件的<see cref="IApplicationBuilder"/></param>
    /// <param name="statistics">这个委托用于统计访客信息，
    /// 它的第一个参数是访客的IP，第二个参数是访客的凭据，第三个参数是一个可以用来请求服务的对象</param>
    /// <returns></returns>
    public static IApplicationBuilder UseVisitorStatistics(this IApplicationBuilder application, Func<IPAddress, ClaimsPrincipal, IServiceProvider, Task> statistics)
        => application.Use(async (http, follow) =>
        {
            await statistics(http.Connection.RemoteIpAddress!, http.User, application.ApplicationServices);
            await follow();
        });

    #endregion
}
