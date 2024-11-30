using System.Buffers.Text;
using System.Text;

namespace System;

/// <summary>
/// 有关网络的扩展方法全部放在这里
/// </summary>
public static partial class ExtendNet
{
    #region 有关字符串互相转换
    #region 转换Base64字符串
    #region 转换为Base64字符串
    /// <summary>
    /// 将字符串转换为Base64字符串
    /// </summary>
    /// <param name="text">封装要转换的字符串的对象</param>
    /// <param name="isUrlBase64">如果这个值为<see langword="true"/>，
    /// 则遵循UrlBase64的格式，否则遵循普通Base64的格式</param>
    /// <returns></returns>
    public static string ToBase64(this StringOperate text, bool isUrlBase64 = true)
    {
        var plainTextBytes = text.Text.ToBytes();
        return isUrlBase64 ?
            Base64Url.EncodeToString(plainTextBytes) :
            Convert.ToBase64String(plainTextBytes);
    }
    #endregion
    #region 从Base64字符串转换
    /// <summary>
    /// 将Base64字符串解码，然后将其转换为字符串
    /// </summary>
    /// <param name="text">封装要转换的字符串的对象</param>
    /// <param name="isUrlBase64">如果这个值为<see langword="true"/>，
    /// 则遵循UrlBase64的格式，否则遵循普通Base64的格式</param>
    /// <returns></returns>
    public static string FromBase64(this StringOperate text, bool isUrlBase64 = true)
    {
        var originalText = text.Text;
        var bytes = isUrlBase64 ?
            Convert.FromBase64String(originalText) :
            Base64Url.DecodeFromChars(originalText);
        return Encoding.UTF8.GetString(bytes);
    }
    #endregion
    #endregion
    #region 转换Hex字符串
    #region 转换为Hex字符串
    /// <summary>
    /// 将字符串转换为Hex字符串，
    /// 例如%FF%AA%BB的形式
    /// </summary>
    /// <param name="text">封装待转换的字符串的对象</param>
    /// <returns></returns>
    public static string ToHex(this StringOperate text)
    {
        var hex = Convert.ToHexString(text.Text.ToBytes());
        var result = hex.Chunk(2).Join(static x => $"{x[0]}{x[1]}", "%");
        return result.IsVoid() ? "" : "%" + result;
    }
    #endregion
    #endregion
    #region 获取终结点
    /// <summary>
    /// 获取Uri路径的终结点，
    /// 它也可以用于获取Uri中静态文件的名称
    /// </summary>
    /// <param name="text">封装要获取终结点的Uri的对象</param>
    /// <returns></returns>
    public static string GetEndPoint(this StringOperate text)
    {
        var uri = text.Text;
        var index = uri.LastIndexOf('/');
        return index == -1 ? "" : uri[(index + 1)..];
    }
    #endregion
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
