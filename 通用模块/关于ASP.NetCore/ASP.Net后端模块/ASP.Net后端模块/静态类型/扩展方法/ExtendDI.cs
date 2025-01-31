using System.NetFrancis;

namespace System;

public static partial class ExtendWebApi
{
    //这个部分类专门用来声明有关依赖注入的扩展方法

    #region 从HttpContext中读取
    /// <summary>
    /// 以范围模式注入一个<see cref="IHostProvide"/>，
    /// 它通过<see cref="HttpContext"/>来提供本机Host地址，
    /// 注意：某些情况下，不能访问<see cref="HttpContext"/>，
    /// 此时会引发异常
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddHostProvideFromHttpContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped(static services =>
        {
            var httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
            var request = httpContextAccessor.HttpContext?.Request ??
            throw new NotSupportedException($"{nameof(IHttpContextAccessor)}服务无法获取{nameof(HttpContext)}");
            var host = $"{request.Scheme}://{request.Host}";
            return CreateNet.HostProvide(host);
        });
        return services;
    }
    #endregion
}
