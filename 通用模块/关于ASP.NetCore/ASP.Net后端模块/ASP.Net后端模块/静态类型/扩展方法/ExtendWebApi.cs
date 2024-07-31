
using System.IOFrancis;
using System.IOFrancis.FileSystem;

namespace System;

/// <summary>
/// 有关WebApi的扩展方法全部放在这里
/// </summary>
public static partial class ExtendWebApi
{
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
