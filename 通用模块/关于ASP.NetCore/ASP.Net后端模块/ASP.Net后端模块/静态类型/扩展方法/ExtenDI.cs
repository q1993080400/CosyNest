using System.NetFrancis;
using System.NetFrancis.Http;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace System;

public static partial class ExtenWebApi
{
    //这个部分类专门用来声明有关依赖注入的扩展方法

    #region 注入UriManager
    /// <summary>
    /// 以范围模式注入一个<see cref="IUriManager"/>，
    /// 它可以用于管理本机Uri，本服务依赖于<see cref="IHttpContextAccessor"/>，
    /// 适用于后端
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddUriManagerServer(this IServiceCollection services)
        => services.AddScoped(x =>
        {
            var httpContext = x.GetService<IHttpContextAccessor>();
            var path = httpContext!.HttpContext!.Request.GetEncodedUrl();
            var uri = new Uri(path).Split().Base;
            return CreateNet.UriManager(uri);
        });
    #endregion
}
