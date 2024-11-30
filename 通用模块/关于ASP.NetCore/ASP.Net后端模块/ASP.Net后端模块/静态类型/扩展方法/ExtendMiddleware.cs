using Microsoft.AspNetCore;

namespace System;

public static partial class ExtendWebApi
{
    //这个部分类专门用来声明有关中间件的扩展方法

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
