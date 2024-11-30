using System.Net;
using System.NetFrancis.Http;

using Microsoft.Extensions.DependencyInjection;

namespace System.NetFrancis.Api;

/// <summary>
/// 这个静态类可以用来帮助创建WebApi
/// </summary>
public static class CreateAPI
{
    #region 创建一个必应图片API接口
    /// <summary>
    /// 创建一个必应图片API接口
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="BingImageAPI(IServiceProvider)"/>
    public static IBingImageAPI ImageAPI(IServiceProvider serviceProvider)
        => new BingImageAPI(serviceProvider);
    #endregion
    #region 创建IP查询接口
    /// <summary>
    /// 创建一个可以用来查询公网IP的接口
    /// </summary>
    /// <param name="serviceProvider">服务提供者对象，它可以用于请求<see cref="IHttpClient"/></param>
    /// <returns></returns>
    public static Func<Task<IPAddress>> PublicNetworkIPAPI(IServiceProvider serviceProvider)
        => async () =>
        {
            var httpClient = serviceProvider.GetRequiredService<IHttpClient>();
            var uri = "https://api.ipify.org/?format=json";
            var request = await httpClient.RequestJsonGet(uri);
            var ip = request.GetValue<string>("ip") ??
            throw new NotSupportedException("没有找到存储IP的字段");
            return IPAddress.Parse(ip);
        };
    #endregion
    #region 创建百度网盘API接口
    /// <summary>
    /// 创建一个<see cref="IBaidupanAPI"/>，
    /// 它可以用于管理百度网盘
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="BaidupanAPI.BaidupanAPI(BaidupanAPIInfo)"/>
    public static IBaidupanAPI BaidupanAPI(BaidupanAPIInfo info)
        => new BaidupanAPI(info);
    #endregion
}
