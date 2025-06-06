﻿using System.Underlying;

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
    #region 生成一个不重复的，符合JS规范的对象名称
    /// <summary>
    /// 生成一个不重复的，符合JS规范的对象名称，
    /// 它可以用于生成JS代码
    /// </summary>
    /// <param name="existing">如果这个值不为<see langword="null"/>，
    /// 则通过这个现有ID生成对象名称</param>
    /// <returns></returns>
    public static string JSObjectName(Guid? existing = null)
    {
        var guid = (existing ?? Guid.CreateVersion7()).ToString();
        return "a" + guid.Remove("-");
    }
    #endregion
}
