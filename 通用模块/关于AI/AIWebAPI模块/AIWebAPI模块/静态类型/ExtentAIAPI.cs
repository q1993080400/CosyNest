using System.AI;
using System.NetFrancis.Http;
using System.NetFrancis.Api;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System;

/// <summary>
/// 有关AI的扩展方法全部放在这里
/// </summary>
public static class ExtentAIAPI
{
    #region 添加百度AI聊天服务
    /// <summary>
    /// 以单例模式添加一个<see cref="IAIChatContext"/>，
    /// 它是用百度接口实现的AI聊天服务
    /// </summary>
    /// <param name="services">服务容器</param>
    /// <returns></returns>
    /// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
    public static IServiceCollection AddAIChatBaiDu(this IServiceCollection services, Func<IHttpClient>? httpClientProvide = null)
    {
        services.AddSingleton(x =>
        {
            var configuration = x.GetRequiredService<IConfiguration>();
            var baiDuConfiguration = configuration.GetSection("AIChatBaiDuConfiguration");
            var appKey = baiDuConfiguration["AppKey"] ??
            throw new KeyNotFoundException("没有找到百度AI聊天服务的AppKey");
            var secretKey = baiDuConfiguration["SecretKey"] ??
            throw new KeyNotFoundException("没有找到百度AI聊天服务的SecretKey");
            return CreateAIAPI.AIChatBaiDu(appKey, secretKey, httpClientProvide);
        });
        return services;
    }
    #endregion
}
