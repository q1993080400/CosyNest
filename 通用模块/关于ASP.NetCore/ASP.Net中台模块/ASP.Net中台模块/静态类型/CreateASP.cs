
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

using System.IOFrancis.Bit;
using System.Maths.Plane;
using System.SafetyFrancis;
using System.Security.Principal;
using System.TreeObject.Json;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个静态类可以用来创建通用的ASP.NET对象，
/// 它们在前端或后端都有用处
/// </summary>
public static class CreateASP
{
    #region 获取公用的服务提供对象
    private static IServiceProvider? SingleServiceProviderField;

    /// <summary>
    /// 获取公用的服务提供对象，
    /// 它可以用于请求单例服务，注意：
    /// 需要手动初始化它，方可使用
    /// </summary>
    public static IServiceProvider SingleServiceProvider
    {
        get => SingleServiceProviderField ??
            throw new NullReferenceException($"{nameof(SingleServiceProvider)}尚未初始化，请自行将它初始化后再使用");
        set => SingleServiceProviderField = value;
    }

    /*问：为什么要使用静态对象，
      而不是依赖注入来访问IServiceProvider？
      答：这是因为在只请求单例服务的情况下，
      IServiceProvider实际上可以也应该静态化，
      它比较方便，而且可以让静态对象也能够请求服务*/
    #endregion
    #region 获取序列化IIdentity的对象
    /// <summary>
    /// 获取一个可以序列化和反序列化<see cref="IIdentity"/>的对象
    /// </summary>
    public static SerializationBase<IIdentity> SerializationIdentity { get; }
        = CreateJson.JsonMap<PseudoIIdentity, IIdentity>
            (value => value is null ? null : new()
            {
                AuthenticationType = value.AuthenticationType,
                Name = value.Name
            },
            vaule => vaule is null ? null : CreateSafety.Identity(vaule.AuthenticationType, vaule.Name));
    #endregion
    #region 获取提取身份验证信息的键名
    /// <summary>
    /// 获取从Cookies中提取身份验证信息的默认键名
    /// </summary>
    public const string AuthenticationKey = "Authentication";
    #endregion
    #region 创建IEnvironmentInfoWeb
    /// <summary>
    /// 通过用户代理字符串创建一个<see cref="IEnvironmentInfoWeb"/>
    /// </summary>
    /// <inheritdoc cref="EnvironmentInfoWeb(string)"/>
    public static IEnvironmentInfoWeb EnvironmentInfo(string userAgent)
        => new EnvironmentInfoWeb(userAgent);
    #endregion
    #region 创建图片提供者对象
    /// <summary>
    /// 创建图片提供者对象，它可以用来管理原图和缩略图
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="ImageProvided.ImageProvided(string, string, Func{IBitRead, ISizePixel, Task{IBitRead}})"/>
    public static IImageProvided ImageProvided(string original, string thumbnail, Func<IBitRead, ISizePixel, Task<IBitRead>> toThumbnail)
        => new ImageProvided(original, thumbnail, toThumbnail);
    #endregion
    #region 私有辅助类
    private class PseudoIIdentity
    {
        public string? AuthenticationType { get; set; }
        public string? Name { get; set; }
    }
    #endregion
}
