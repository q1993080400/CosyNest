using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

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
    #region 创建文件路径协议
    /// <summary>
    /// 创建文件路径协议，它是一个委托元组，
    /// 它的第一个项可以编码文件路径，第二个项可以解码文件路径
    /// </summary>
    public static (Func<IEnumerable<MediaPathGenerateParameters>, IEnumerable<MediaServerPosition>> Generate, Func<IEnumerable<string>, IEnumerable<MediaSource>> Analysis) MediaPathProtocol { get; }
        = (Components.MediaPathProtocol.Generate, Components.MediaPathProtocol.Analysis);
    #endregion
}
