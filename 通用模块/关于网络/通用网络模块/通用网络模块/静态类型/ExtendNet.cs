using System.Text;

namespace System;

/// <summary>
/// 有关网络的扩展方法全部放在这里
/// </summary>
public static partial class ExtendNet
{
    #region 有关字符串互相转换
    #region 有关路径转换
    #region 说明文档
    /*问：本地路径和Uri路径有什么区别？
      答：本地路径是在服务器本机上的路径，它以左斜杠作为分隔符，
      Uri路径是客户端从服务端获取资源的路径，它以右斜杠作为分隔符*/
    #endregion
    #region Uri路径和本地路径之间的转换
    #region 将Uri路径转换为本地路径
    /// <summary>
    /// 将Uri路径转换为本地路径
    /// </summary>
    /// <param name="localPath">封装待转换的Uri路径的对象</param>
    /// <param name="addWwwRoot">如果这个值为<see langword="true"/>，
    /// 则会在路径前面添加wwwroot文件夹，使这个路径可以直接通过与IO有关的API进行访问</param>
    /// <returns></returns>
    public static string ToLocalPath(this StringOperate localPath, bool addWwwRoot = false)
    {
        var path = (Uri.TryCreate(localPath.Text, UriKind.Absolute, out var url) ?
            url.LocalPath : localPath.Text.Replace('/', '\\')).TrimStart('\\');
        return addWwwRoot ? Path.Combine("wwwroot", path) : path;
    }
    #endregion
    #region 将本地转换为Uri路径
    /// <summary>
    /// 将本地转换为Uri路径
    /// </summary>
    /// <param name="localPath">封装待转换的本地路径的对象</param>
    /// <returns></returns>
    public static string ToUriPath(this StringOperate localPath)
        => localPath.ToVirtualPath().Replace("\\", "/");
    #endregion
    #endregion
    #region 真实路径和虚拟路径之间的转换
    #region 将真实路径转换为虚拟路径
    /// <summary>
    /// 将真实的物理路径转换为相对wwwroot文件夹的虚拟路径
    /// </summary>
    /// <param name="localPath">封装待转换的真实路径的对象</param>
    /// <returns></returns>
    public static string ToVirtualPath(this StringOperate localPath)
    {
        var text = localPath.Text;
        const string wwwroot = "wwwroot";
        var index = text.IndexOf("wwwroot");
        return index < 0 ?
            text :
            text[(index + wwwroot.Length)..];
    }
    #endregion
    #region 将虚拟路径转换为真实路径
    /// <summary>
    /// 将相对于wwwroot文件夹的虚拟路径转换为真实路径
    /// </summary>
    /// <param name="realityPath">封装待转换的虚拟路径的对象</param>
    /// <returns></returns>
    public static string ToRealityPath(this StringOperate realityPath)
        => Path.Combine("wwwroot", realityPath.Text.TrimStart('\\'));
    #endregion
    #endregion
    #endregion
    #endregion
}
