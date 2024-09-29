using Microsoft.AspNetCore;

namespace System;

public static partial class ExtendWebApi
{
    //这个部分类专门用来声明有关中间件的扩展方法

    #region 添加访客统计中间件
    /// <summary>
    /// 添加一个访客统计中间件，
    /// 它可以将访客日志写入数据库
    /// </summary>
    /// <typeparam name="Log">访问日志的类型</typeparam>
    /// <param name="application">待添加中间件的<see cref="IApplicationBuilder"/></param>
    /// <param name="createLog">这个委托的参数是当前<see cref="HttpContext"/>对象，
    /// 返回值是创建好的访问日志</param>
    /// <returns></returns>
    public static IApplicationBuilder UseAccessStatistics<Log>(this IApplicationBuilder application, Func<HttpContext, Log> createLog)
        where Log : class
        => application.Use(async (http, follow) =>
        {
            await using var pipe = http.RequestServices.RequiredDataPipe();
            var log = createLog(http);
            await pipe.Push(context => context.AddOrUpdate([log]));
            await follow();
        });
    #endregion
    #region 添加PWA版本中间件
    /// <summary>
    /// 添加一个中间件，它能够使响应携带PWA版本
    /// </summary>
    /// <param name="applicationBuilder">待添加中间件的<see cref="IApplicationBuilder"/></param>
    /// <param name="manifestPath">manifest.webmanifest文件的位置</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public static IApplicationBuilder UsePWAVersion(this IApplicationBuilder applicationBuilder, string manifestPath = @"wwwroot\manifest.webmanifest")
    {
        var pwaVersion = ToolPWA.GetManifest(manifestPath)[ToolPWA.KeyVersion]?.ToString() ??
            throw new KeyNotFoundException("未找到PWA版本");
        return applicationBuilder.Use(async (httpContext, next) =>
        {
            httpContext.Response.Headers.TryAdd("PWAVersion", pwaVersion);
            await next();
        });
    }
    #endregion
}
