using System.IOFrancis.FileSystem;

namespace Microsoft.AspNetCore;

/// <summary>
/// 本类型封装了一个文件路径，
/// 它在后端是文件路径，
/// 当传递到前端时，就会变成Uri
/// </summary>
public record FileSource
{
    #region 静态方法
    #region 返回返回媒体的类型
    /// <summary>
    /// 根据路径，返回媒体的类型
    /// </summary>
    /// <param name="uriOrPath">媒体文件的路径或Uri</param>
    public static FileSourceType GetMediaSourceType(string uriOrPath)
        => uriOrPath switch
        {
            var uri when FileTypeCom.WebImage.IsCompatible(uri) => FileSourceType.WebImage,
            var uri when FileTypeCom.WebVideo.IsCompatible(uri) => FileSourceType.WebVideo,
            _ => FileSourceType.File
        };
    #endregion
    #endregion
    #region 文件本来的名字
    /// <summary>
    /// 获取文件的本名，
    /// 也就是它未上传之前本来的名字
    /// </summary>
    public required string TrueName { get; init; }
    #endregion
    #region 媒体路径
    /// <summary>
    /// 获取媒体本体的路径，
    /// 它可以是图片，视频，也可以是其他文件
    /// </summary>
    public required string FilePath { get; init; }
    #endregion
    #region 文件信息
    /// <summary>
    /// 获取一个元组，
    /// 它的项分别指示文件的名称，扩展名，还有文件的全名
    /// </summary>
    public (string Simple, string? Extended, string FullName) FileInfo
        => ToolPath.SplitFilePath(FilePath);
    #endregion
    #region 返回返回媒体的类型
    /// <summary>
    /// 返回媒体的类型
    /// </summary>
    public FileSourceType MediaSourceType
        => GetMediaSourceType(FilePath);
    #endregion
    #region 转换文件路径
    #region 转换为路径模式
    /// <summary>
    /// 将本对象转换为路径模式，
    /// 它适用于服务端
    /// </summary>
    /// <returns></returns>
    public virtual FileSource ToPathMod()
    {
        var path = FilePath.Op().ToLocalPath();
        return new()
        {
            FilePath = path,
            TrueName = TrueName,
        };
    }
    #endregion
    #region 转换为Uri模式
    /// <summary>
    /// 将本对象转换为Uri模式，
    /// 它适用于客户端
    /// </summary>
    /// <returns></returns>
    public virtual FileSource ToUriMod()
    {
        var path = FilePath.Op().ToUriPath();
        return new()
        {
            FilePath = path,
            TrueName = TrueName,
        };
    }
    #endregion
    #endregion
    #region 删除文件
    /// <summary>
    /// 将这个文件删除，
    /// 本方法只能在服务器上调用
    /// </summary>
    public virtual void Delete()
    {
        File.Delete(FilePath.Op().ToLocalPath(true));
    }
    #endregion
}
