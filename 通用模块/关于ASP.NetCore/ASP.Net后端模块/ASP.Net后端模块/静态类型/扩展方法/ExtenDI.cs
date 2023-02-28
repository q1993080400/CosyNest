using System.NetFrancis;
using System.NetFrancis.Http;

using Microsoft.AspNetCore.Http.Extensions;

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
            var httpContext = x.GetRequiredService<IHttpContextAccessor>();
            var path = httpContext.HttpContext?.Request.GetEncodedUrl() ?? "http://127.0.0.1";
            return CreateNet.UriManager(path);
        });

    /*此API存在潜在问题：
      当不存在HttpContext的时候，
      本函数返回一个回退地址，这个可能会产生问题，
      但是，考虑到当不存在HttpContext时，基本不会用到服务，
      所以本函数出现问题的概率应该相当小*/
    #endregion
}
