
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

using System.IOFrancis.Bit;
using System.Maths.Plane;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个静态类可以用来创建通用的ASP.NET对象，
/// 它们在前端或后端都有用处
/// </summary>
public static class CreateASP
{
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
}
