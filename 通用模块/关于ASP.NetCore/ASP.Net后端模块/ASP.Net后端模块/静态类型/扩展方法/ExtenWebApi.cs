﻿
using System.IOFrancis;
using System.IOFrancis.FileSystem;

namespace System;

/// <summary>
/// 有关WebApi的扩展方法全部放在这里
/// </summary>
public static partial class ExtenWebApi
{
    #region 关于LinkGenerator
    #region 通过控制器和操作名称获取Uri
    /// <summary>
    /// 通过控制器和操作名称，获取相对于主机地址的Uri，
    /// 如果无法创建Uri，则返回<see langword="null"/>
    /// </summary>
    /// <typeparam name="Controller">控制器的类型</typeparam>
    /// <param name="generator">用于生成Uri的约定</param>
    /// <param name="action">操作的名称</param>
    /// <returns></returns>
    public static string? GetPathByAction<Controller>(this LinkGenerator generator, string action)
        where Controller : ControllerBase
        => generator.GetPathByAction(action, typeof(Controller).Name.Remove("Controller"));
    #endregion
    #endregion
    #region 获取Web根目录对象
    /// <summary>
    /// 获取Web根目录的<see cref="IDirectory"/>对象
    /// </summary>
    /// <param name="webHostEnvironment">该对象被用来获取Web根目录的路径</param>
    /// <returns></returns>
    public static IDirectory WebRootPath(this IWebHostEnvironment webHostEnvironment)
        => CreateIO.Directory(webHostEnvironment.WebRootPath);
    #endregion
}
